using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
namespace PostService.Infrastructure.Extensions
{
    public static class InfrastructureExtension
    {
        public static IServiceCollection AddInfrastructureExtension(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            services.AddHostedExtension();
            services.AddIntegrations(configuration);
            services.AddRedis(configuration);
            services.AddRabbitMQConnection(configuration);
            services.AddDBContextExtension(configuration);
            services.AddDependencyInjection();
            return services;
        }
    }
}