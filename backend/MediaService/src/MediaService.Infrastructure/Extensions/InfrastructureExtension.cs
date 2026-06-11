using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace MediaService.Infrastructure.Extensions
{
    public static class InfrastructureExtension
    {
        public static IServiceCollection AddInfrastructureExtension(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            // services.AddHostedExtension();
            // services.AddIntegrations(configuration);
            services.AddRedis(configuration);
            services.AddRabbitMQConnection(configuration);
            services.AddDBContextExtension(configuration);
            services.AddDependencyInjection();
            return services;
        }
    }
}