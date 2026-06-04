using CertificateService.Application.Interfaces;
using CertificateService.Application.Services;
using CertificateService.Application.UseCases.CertificatePosts;
using CertificateService.Application.UseCases.CertificatePosts.Interfaces;
using CertificateService.Application.UseCases.Certificates;
using CertificateService.Application.UseCases.Certificates.Interfaces;
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
            services.AddScoped<IMediaFilesServices, MediaFilesServices>();

            services.AddScoped<ICertificateCacheServices, CertificateCacheServices>();
            services.AddScoped<ICertificatePostsCacheServices, CertificatePostCacheServices>();

            services.AddScoped<IAddCertificate, AddCertificate>();
            services.AddScoped<IUpdateCertificate, UpdateCertificate>();
            services.AddScoped<IRemoveCertificate, RemoveCertificate>();
            services.AddScoped<IAddRelatePost, AddRelatePost>();
            services.AddScoped<IRemoveRelatePost, RemoveRelatePost>();
            return services;
        }
    }
}