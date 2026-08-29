using CertificateService.Application.Caching.Interfaces;
using CertificateService.Application.Interfaces;
using CertificateService.Application.Services;
using CertificateService.Application.UseCases.Certificates;
using CertificateService.Application.UseCases.Certificates.Interfaces;
using CertificateService.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using CertificateService.Application.Caching;
namespace CertificateService.Application.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjectionExtensions(
            this IServiceCollection services
        )
        {
            services.AddScoped<ICertificateServices, CertificateServices>();
            services.AddScoped<IMediaProjectionServices, MediaProjectionServices>();

            services.AddScoped<ICertificateCacheServices, CertificateCacheServices>();
            services.AddScoped<IPostCacheServices, PostCacheServices>();

            services.AddScoped<IAddCertificate, AddCertificate>();
            services.AddScoped<IUpdateCertificate, UpdateCertificate>();
            services.AddScoped<IRemoveCertificate, RemoveCertificate>();
            return services;
        }
    }
}