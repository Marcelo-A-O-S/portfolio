using Microsoft.Extensions.DependencyInjection;

namespace CommentService.Infrastructure.Extensions
{
    public static class InfrastructureExtension
    {
        public static IServiceCollection AddInfrastructureExtensions(
            this IServiceCollection services
        ){
            
            return services;
        }
    }
}