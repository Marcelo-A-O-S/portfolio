using Microsoft.Extensions.DependencyInjection;

namespace CommentService.Application.Extensions
{
    public static class ApplicationExtension
    {
        public static IServiceCollection AddApplicationExtensions(
            this IServiceCollection services
        )
        {
            services.AddDependencyInjectionExtension();
            return services;
        }
    }
}