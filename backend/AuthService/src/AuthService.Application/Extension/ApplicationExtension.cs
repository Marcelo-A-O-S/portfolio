using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace AuthService.Application.Extension
{
    public static class ApplicationExtension
    {
        public static IServiceCollection AddApplicationExtension(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            services.AddDependencyInjection();
            services.AddInternalConfigurations(configuration);
            return services;
        }
    }
}