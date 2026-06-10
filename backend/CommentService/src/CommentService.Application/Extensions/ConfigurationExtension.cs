using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CommentService.Application.Configurations;
namespace CommentService.Application.Extensions
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