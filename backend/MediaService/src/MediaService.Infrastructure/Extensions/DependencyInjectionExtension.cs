using MediaService.Domain.Entities;
using MediaService.Domain.Interfaces;
using MediaService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
namespace MediaService.Infrastructure.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjection(
            this IServiceCollection services
        )
        {
            services.AddScoped<IGenerics<MediaFile>, Generics<MediaFile>>();

            services.AddScoped<IMediaFileRepository, MediaFileRepository>();
            return services;
        }
    }
}