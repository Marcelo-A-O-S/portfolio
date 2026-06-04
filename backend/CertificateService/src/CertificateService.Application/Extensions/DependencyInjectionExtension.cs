using CertificateService.Application.Interfaces;
using CertificateService.Application.Services;
using CertificateService.Domain.Entities;
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
            services.AddScoped<ICertificatePostsServices, CertificatePostsServices>();

            services.AddScoped<ICertificateCacheServices, CertificateCacheServices>();
            services.AddScoped<ICertificatePostsCacheServices, CertificatePostCacheServices>();
            return services;
        }
    }
}