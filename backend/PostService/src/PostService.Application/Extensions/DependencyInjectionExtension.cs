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
            services.AddScoped<IPostCategoryServices, PostCategoryServices>();
            services.AddScoped<IPostServices, PostServices>();
            services.AddScoped<IPostToolServices, PostToolServices>();
            services.AddScoped<ISectionServices, SectionServices>();
            services.AddScoped<IToolsServices, ToolsServices>();
            return services;
        }
    }
}