using CertificateService.Application.Interfaces;
using CertificateService.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CertificateService.Application.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjectionExtensions(
            this IServiceCollection services
        )
        {
            services.AddScoped<ICertificateServices, CertificateServices>();
            return services;
        }
    }
}