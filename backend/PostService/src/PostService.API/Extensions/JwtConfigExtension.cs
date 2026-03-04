using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace PostService.API.Extensions
{
    public static class JwtConfigExtension
    {
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            var secret = configuration.GetSection("JWT:Secret").Value;
            var IssuerSecret = configuration.GetSection("JWT:Issuer").Value;
            if(string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(IssuerSecret))
            {
                throw new InvalidOperationException("Chaves não configuradas corretamente.");
            }
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidIssuer = IssuerSecret,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                };
            });
            return services;
        }
    }
}