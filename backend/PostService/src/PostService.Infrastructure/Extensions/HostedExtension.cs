using Microsoft.Extensions.DependencyInjection;
using PostService.Infrastructure.Messaging.Consumers;
namespace PostService.Infrastructure.Extensions
{
    public static class HostedExtension
    {
        public static IServiceCollection AddHostedExtension(
            this IServiceCollection services
        )
        {
            services.AddHostedService<PostConsumer>();
            services.AddHostedService<ToolConsumer>();
            services.AddHostedService<UserConsumer>();
            return services;
        }
    }
}