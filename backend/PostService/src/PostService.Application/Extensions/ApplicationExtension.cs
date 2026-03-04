using Microsoft.Extensions.DependencyInjection;

namespace PostService.Application.Extensions
{
    public static class ApplicationExtension
    {
        public static IServiceCollection AddApplicationExtensions(
            this IServiceCollection services
        )
        {
            services.AddDependencyInjection();
            return services;
        }
    }
}