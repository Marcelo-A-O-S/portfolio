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
            services.Configure<InternalClientOptions>(configuration.GetSection("InternalClientOptions"));
            return services;
        }
    }
}