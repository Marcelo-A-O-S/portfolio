using AuthService.Infrastructure.Jobs;
using AuthService.Infrastructure.Messaging.Consumers;
using Microsoft.Extensions.DependencyInjection;
namespace AuthService.Infrastructure.Extensions
{
    public static class HostedExtension
    {
        public static IServiceCollection AddHostedExtension(
            this IServiceCollection services
        )
        {
            services.AddHostedService<OutboxProcessor>();
            services.AddHostedService<OutboxCleanupJob>();
            services.AddHostedService<RefreshTokenCleanupJob>();
            services.AddHostedService<UserConsumer>();
            return services;
        } 
    }
}