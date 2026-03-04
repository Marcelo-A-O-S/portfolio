using Microsoft.Extensions.DependencyInjection;

namespace CertificateService.Application.Extensions
{
    public static class ApplicationExtension
    {
        public static IServiceCollection AddApplicationExtensions(
            this IServiceCollection services
        )
        {
            services.AddDependencyInjectionExtensions();
            return services;
        }
    }
}