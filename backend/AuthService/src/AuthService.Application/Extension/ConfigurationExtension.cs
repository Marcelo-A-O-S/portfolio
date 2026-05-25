using AuthService.Application.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace AuthService.Application.Extension
{
    public static class ConfigurationExtension
    {
        public static IServiceCollection AddInternalConfigurations(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            services.Configure<InternalClientOptions>(configuration.GetSection("InternalClients"));
            services.Configure<JwtBearerOptions>(configuration.GetSection("JWT"));
            services.Configure<InternalJWTOptions>(configuration.GetSection("InternalJWT"));
            return services;
        }
    }
}