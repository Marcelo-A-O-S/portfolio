using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CertificateService.Application.Extensions
{
    public static class ApplicationExtension
    {
        public static IServiceCollection AddApplicationExtensions(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            services.AddDependencyInjectionExtensions();
            services.AddInternalConfigurations(configuration);
            return services;
        }
    }
}