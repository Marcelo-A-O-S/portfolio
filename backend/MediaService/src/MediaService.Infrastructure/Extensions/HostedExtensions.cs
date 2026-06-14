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
            services.AddHostedService<ToolConsumer>();
            services.AddHostedService<PostConsumer>();
            services.AddHostedService<CertficateConsumer>();
            services.AddHostedService<CleanupMediaJob>();
            return services;
        }
    }
}