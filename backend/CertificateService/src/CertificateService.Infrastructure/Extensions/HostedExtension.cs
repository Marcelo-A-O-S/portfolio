using Microsoft.Extensions.DependencyInjection;
using CertificateService.Infrastructure.Jobs;
namespace CertificateService.Infrastructure.Extensions
{
    public static class HostedExtension
    {
        public static IServiceCollection AddHostedExtension(
            this IServiceCollection services
        )
        {
            services.AddHostedService<CleanupMediaJob>();
            return services;
        }
    }
}