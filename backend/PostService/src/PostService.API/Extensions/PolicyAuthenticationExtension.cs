using Microsoft.IdentityModel.JsonWebTokens;
namespace PostService.API.Extensions
{
    public static class PolicyAuthenticationExtension
    {
        public static IServiceCollection AddPolicyAuthentications(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("UsersRead", policy =>
                {
                    policy.RequireClaim("scope", "users.read");
                    policy.RequireClaim("client_type", "internal");
                    policy.RequireClaim(JwtRegisteredClaimNames.Aud, "auth-internal");
                });
            });
            return services;
        }
    }
}