using Microsoft.Extensions.DependencyInjection;
using MediaService.Infrastructure.Jobs;
namespace MediaService.Infrastructure.Extensions
{
    public static class HostedExtensions
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