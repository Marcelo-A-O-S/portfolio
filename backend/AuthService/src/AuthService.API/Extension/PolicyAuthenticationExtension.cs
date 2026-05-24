using Microsoft.IdentityModel.JsonWebTokens;
namespace AuthService.API.Extension
{
    public static class PolicyAuthenticationExtension
    {
        public static IServiceCollection AddPolicyAuthentications(
            IServiceCollection services
        )
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("InternalService", policy =>
                {
                    policy.RequireClaim("scope","users.read");
                    policy.RequireClaim(JwtRegisteredClaimNames.Aud, "auth-internal");
                });
            });
            return services;
        }
    }
}