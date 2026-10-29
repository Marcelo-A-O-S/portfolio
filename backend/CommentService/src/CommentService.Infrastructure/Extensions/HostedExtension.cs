using Microsoft.Extensions.DependencyInjection;
using CommentService.Infrastructure.Messaging.Consumers;
using CommentService.Infrastructure.Jobs;
namespace CommentService.Infrastructure.Extensions
{
    public static class HostedExtension
    {
         public static IServiceCollection AddHostedExtension(
            this IServiceCollection services
        )
        {
            services.AddHostedService<OutboxProcessor>();
            services.AddHostedService<OutboxCleanupJob>();
            services.AddHostedService<CommentConsumer>();
            services.AddHostedService<PostConsumer>();
            services.AddHostedService<UserConsumer>();
            
            return services;
        }
    }
}