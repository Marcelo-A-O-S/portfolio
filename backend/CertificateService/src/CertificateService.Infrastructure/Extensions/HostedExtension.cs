using Microsoft.Extensions.DependencyInjection;
using CertificateService.Infrastructure.Jobs;
using CertificateService.Infrastructure.Messaging.Consumers;
namespace CertificateService.Infrastructure.Extensions
{
    public static class HostedExtension
    {
        public static IServiceCollection AddHostedExtension(
            this IServiceCollection services
        )
        {
            services.AddHostedService<CleanupMediaJob>();
            services.AddHostedService<CertificateConsumer>();
            return services;
        }
    }
}