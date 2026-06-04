using CertificateService.Domain.Interfaces;
using CertificateService.Infrastructure.Repositories;
using CertificateService.Infrastructure.Workers;
using Microsoft.Extensions.DependencyInjection;
using CertificateService.Domain.Entities;
namespace CertificateService.Infrastructure.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjectionExtensions(
            this IServiceCollection services
        )
        {
            services.AddScoped<IGenerics<Certificate>, Generics<Certificate>>();
            services.AddScoped<IGenerics<CertificatePost>, Generics<CertificatePost>>();
            services.AddScoped<IGenerics<MediaFile>, Generics<MediaFile>>();

            services.AddScoped<ICertificateRepository, CertificateRepository>();
            services.AddScoped<ICertificatePostsRepository, CertificatePostsRepository>();
            services.AddScoped<IMediaFilesRepository, MediaFilesRepository>();

            services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();
            return services;
        }
    }
}