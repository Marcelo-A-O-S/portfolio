using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace PostService.Application.Extensions
{
    public static class ApplicationExtension
    {
        public static IServiceCollection AddApplicationExtensions(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            services.AddDependencyInjection();
            services.AddInternalConfigurations(configuration);
            return services;
        }
    }
}