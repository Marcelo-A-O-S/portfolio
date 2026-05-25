using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using PostService.Application.Configurations;

namespace PostService.Application.Extensions
{
    public static class ConfigurationExtension
    {
        public static IServiceCollection AddInternalConfigurations(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            services.Configure<InternalClient>(configuration.GetSection("InternalClients:PostService"));
            return services;
        }
    }
}