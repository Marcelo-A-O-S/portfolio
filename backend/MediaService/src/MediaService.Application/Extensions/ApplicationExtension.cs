using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace MediaService.Application.Extensions
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