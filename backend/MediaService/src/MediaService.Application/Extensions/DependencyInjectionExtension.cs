using MediaService.Application.Caching.Certificate;
using MediaService.Application.Caching.Post;
using MediaService.Application.Caching.Tool;
using MediaService.Application.Interfaces;
using MediaService.Application.Services;
using Microsoft.Extensions.DependencyInjection;
namespace MediaService.Application.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjection(
            this IServiceCollection services
        )
        {
            services.AddScoped<IMediaFileServices, MediaFileServices>();

            services.AddScoped<IToolCacheServices, ToolCacheServices>();
            services.AddScoped<IPostCacheServices, PostCacheServices>();
            services.AddScoped<ICertificateCacheServices, CertificateCacheServices>();
            return services;
        }
    }
}