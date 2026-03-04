using CertificateService.Domain.Interfaces;
using CertificateService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using CertificateService.Domain.Entities;

namespace CertificateService.Infrastructure.Context
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjectionExtensions(
            this IServiceCollection services
        )
        {
            services.AddScoped<IGenerics<Certificate>, Generics<Certificate>>();
            services.AddScoped<ICertificateRepository, CertificateRepository>();
            return services;
        }
    }
}