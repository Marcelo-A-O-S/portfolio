using MediaService.Domain.Entities;
using MediaService.Domain.Interfaces;
using MediaService.Infrastructure.Messaging.Handlers.Interfaces;
using MediaService.Infrastructure.Messaging.Handlers;
using MediaService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MediaService.Infrastructure.Workers;
namespace MediaService.Infrastructure.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjection(
            this IServiceCollection services
        )
        {
            services.AddScoped<IGenerics<Media>, Generics<Media>>();
            services.AddScoped<IMediaRepository, MediaRepository>();
            services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();
            services.AddSingleton<IMediaFileProjectionHandler, MediaFileProjectionHandler>();
            return services;
        }
    }
}