using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace CommentService.Infrastructure.Extensions
{
    public static class InfrastructureExtension
    {
        public static IServiceCollection AddInfrastructureExtensions(
            this IServiceCollection services, IConfiguration configuration
        ){
            services.AddIntegrations(configuration);
            services.AddRedis(configuration);
            services.AddRabbitMQConnection(configuration);
            services.AddDBContextExtension(configuration);
            services.AddDependencyInjectionExtension();
            services.AddHostedExtension();
            return services;
        }
    }
}