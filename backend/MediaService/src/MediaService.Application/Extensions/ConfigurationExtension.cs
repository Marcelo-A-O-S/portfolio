using MediaService.Application.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace MediaService.Application.Extensions
{
    public static class ConfigurationExtension
    {
        public static IServiceCollection AddInternalConfigurations(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            services.Configure<InternalClientOptions>(configuration.GetSection("InternalClientOptions"));
            services.Configure<RedisOptions>(configuration.GetSection("Redis"));
            return services;
        }
    }
}