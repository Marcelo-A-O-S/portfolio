using MediaService.Application.Caching.Certificate;
using MediaService.Application.Caching.Post;
using MediaService.Application.Caching.Tool;
using MediaService.Application.Interfaces;
using MediaService.Application.Services;
using MediaService.Application.UseCases.MediaFiles;
using MediaService.Application.UseCases.MediaFiles.Interfaces;
using MediaService.Application.Validators;
using MediaService.Application.Validators.Interfaces;
using Microsoft.Extensions.DependencyInjection;
namespace MediaService.Application.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjection(
            this IServiceCollection services
        )
        {
            services.AddScoped<IMediaServices, MediaServices>();

            services.AddScoped<IToolCacheServices, ToolCacheServices>();
            services.AddScoped<IPostCacheServices, PostCacheServices>();
            services.AddScoped<ICertificateCacheServices, CertificateCacheServices>();

            services.AddScoped<IMediaValidationServices, MediaValidationServices>();
            
            services.AddScoped<IAddMediaFile, AddMediaFile>();
            services.AddScoped<IRemoveMediaFile, RemoveMediaFile>();
            return services;
        }
    }
}