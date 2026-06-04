using Microsoft.Extensions.DependencyInjection;
using CommentService.Infrastructure.Messaging.Consumers;
namespace CommentService.Infrastructure.Extensions
{
    public static class HostedExtension
    {
         public static IServiceCollection AddHostedExtension(
            this IServiceCollection services
        )
        {
            services.AddHostedService<CommentConsumer>();
            return services;
        }
    }
}