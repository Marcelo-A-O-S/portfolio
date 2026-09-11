using CertificateService.Application.Caching.Interfaces;
using CertificateService.Application.Interfaces;
using CertificateService.Application.Services;
using CertificateService.Application.UseCases.Certificates;
using CertificateService.Application.UseCases.Certificates.Interfaces;
using CertificateService.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using CertificateService.Application.Caching;
using CertificateService.Application.Validators.Interfaces;
using CertificateService.Application.Validators;
namespace CertificateService.Application.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjectionExtensions(
            this IServiceCollection services
        )
        {
            services.AddScoped<ICertificateServices, CertificateServices>();
            services.AddScoped<ICertificateContentServices, CertificateContentServices>();
            services.AddScoped<ILanguageProjectionServices, LanguageProjectionServices>();
            services.AddScoped<IMediaProjectionServices, MediaProjectionServices>();
            services.AddScoped<IPostProjectionServices, PostProjectionServices>();

            services.AddScoped<ICertificateCacheServices, CertificateCacheServices>();
            services.AddScoped<IPostCacheServices, PostCacheServices>();

            services.AddScoped<IValidationServices, ValidationServices>();

            services.AddScoped<IAddCertificate, AddCertificate>();
            services.AddScoped<IUpdateCertificate, UpdateCertificate>();
            services.AddScoped<IRemoveCertificate, RemoveCertificate>();
            services.AddScoped<IAddPostProjectionCertificate, AddPostProjectionCertificate>();
            return services;
        }
    }
}