using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CertificateService.API.Extensions
{
    public static class JwtConfigExtension
    {
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            var secret = configuration.GetSection("JWT:Secret").Value;
            var IssuerSecret = configuration.GetSection("JWT:Issuer").Value;
            var secretInternal = configuration.GetSection("InternalJWT:Secret").Value;
            var IssuerSecretInternal = configuration.GetSection("InternalJWT:Issuer").Value;
            if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(IssuerSecret) ||
                string.IsNullOrEmpty(secretInternal) || string.IsNullOrEmpty(IssuerSecretInternal))
            {
                throw new InvalidOperationException("Chaves não configuradas corretamente.");
            }
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer("UserJwt",options =>
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
            }).AddJwtBearer("InternalJwt",options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidIssuer = IssuerSecretInternal,
                    ValidateAudience = true,
                    ValidAudience = "auth-internal",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretInternal))
                };
            });
            return services;
        }
    }
}