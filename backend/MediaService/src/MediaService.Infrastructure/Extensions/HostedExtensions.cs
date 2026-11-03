using Microsoft.Extensions.DependencyInjection;
using MediaService.Infrastructure.Jobs;
using MediaService.Infrastructure.Messaging.Consumers;
namespace MediaService.Infrastructure.Extensions
{
    public static class HostedExtensions
    {
        public static IServiceCollection AddHostedExtension(
            this IServiceCollection services
        )
        {
            services.AddHostedService<CleanupMediaJob>();
            services.AddHostedService<OutboxProcessor>();
            services.AddHostedService<OutboxRecovery>();
            services.AddHostedService<OutboxCleanupJob>();
            services.AddHostedService<ToolConsumer>();
            services.AddHostedService<PostConsumer>();
            services.AddHostedService<CertficateConsumer>();
            
            return services;
        }
    }
}