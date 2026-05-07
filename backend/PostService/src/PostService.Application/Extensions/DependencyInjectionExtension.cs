using Microsoft.Extensions.DependencyInjection;
using PostService.Application.Interfaces;
using PostService.Application.Services;
using PostService.Application.UseCases.Categories.Interfaces;
using PostService.Application.UseCases.Categories;
using PostService.Application.UseCases.Tools;
using PostService.Application.UseCases.Tools.Interfaces;

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
            services.AddScoped<IMediaFileServices, MediaFileServices>();
            services.AddScoped<IFileServices, FileServices>();

            services.AddScoped<ICreateTool, CreateTool>();
            services.AddScoped<IUpdateTool, UpdateTool>();
            services.AddScoped<IDeleteTool, DeleteTool>();
            services.AddScoped<ICreateCategory, CreateCategory>();
            services.AddScoped<IUpdateCategory, UpdateCategory>();
            services.AddScoped<IDeleteCategory, DeleteCategory>();
            return services;
        }
    }
}