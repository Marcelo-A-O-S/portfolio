using Microsoft.Extensions.DependencyInjection;
using PostService.Application.Interfaces;
using PostService.Application.Services;

namespace PostService.Application.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjection(
            this IServiceCollection services
        )
        {
            services.AddScoped<ICategoryServices, CategoryServices>();
            services.AddScoped<ILikeServices, LikeServices>();
            services.AddScoped<IPostServices, PostServices>();
            services.AddScoped<IToolsServices, ToolsServices>();
            services.AddScoped<ICategoryServices, CategoryServices>();
            services.AddScoped<ICategoryContentServices, CategoryContentServices>();
            services.AddScoped<IToolContentServices, ToolContentServices>();
            services.AddScoped<IPostContentServices, PostContentServices>();
            services.AddScoped<ILanguageServices, LanguageServices>();
            services.AddScoped<IFileServices, FileServices>();
            return services;
        }
    }
}