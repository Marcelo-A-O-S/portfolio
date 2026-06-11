using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommentService.Application.Extensions
{
    public static class ApplicationExtension
    {
        public static IServiceCollection AddApplicationExtensions(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            services.AddDependencyInjectionExtension();
            services.AddInternalConfigurations(configuration);
            return services;
        }
    }
}