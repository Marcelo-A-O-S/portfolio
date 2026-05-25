using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace CommentService.Infrastructure.Extensions
{
    public static class InfrastructureExtension
    {
        public static IServiceCollection AddInfrastructureExtensions(
            this IServiceCollection services, IConfiguration configuration
        ){
            services.AddRedis(configuration);
            services.AddDependencyInjectionExtension();
            return services;
        }
    }
}