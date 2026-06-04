using Microsoft.Extensions.DependencyInjection;
using PostService.Application.Interfaces;
using PostService.Application.Services;
using PostService.Application.UseCases.Categories.Interfaces;
using PostService.Application.UseCases.Categories;
using PostService.Application.UseCases.Tools;
using PostService.Application.UseCases.Tools.Interfaces;
using PostService.Application.UseCases.Languages.Interfaces;
using PostService.Application.UseCases.Languages;
using PostService.Application.UseCases.Projects.Interfaces;
using PostService.Application.UseCases.Projects;
using PostService.Application.UseCases.Likes.Interfaces;
using PostService.Application.UseCases.Likes;
using PostService.Application.UseCases.InternalProject.Interfaces;
using PostService.Application.UseCases.InternalProject;
namespace PostService.Application.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjection(
            this IServiceCollection services
        )
        {
            services.AddScoped<ILikeServices, LikeServices>();
            services.AddScoped<IPostServices, PostServices>();
            services.AddScoped<IPostContentServices, PostContentServices>();
            services.AddScoped<ICategoryServices, CategoryServices>();
            services.AddScoped<ICategoryContentServices, CategoryContentServices>();
            services.AddScoped<IToolsServices, ToolsServices>();
            services.AddScoped<IToolContentServices, ToolContentServices>();
            services.AddScoped<ILanguageServices, LanguageServices>();
            services.AddScoped<IMediaFileServices, MediaFileServices>();
            services.AddScoped<IFileServices, FileServices>();

            services.AddScoped<IUserCacheServices, UserCacheServices>();
            services.AddScoped<ILikeCacheServices, LikeCacheServices>();

            services.AddScoped<ICreateTool, CreateTool>();
            services.AddScoped<IUpdateTool, UpdateTool>();
            services.AddScoped<IDeleteTool, DeleteTool>();
            services.AddScoped<ICreateCategory, CreateCategory>();
            services.AddScoped<IUpdateCategory, UpdateCategory>();
            services.AddScoped<IDeleteCategory, DeleteCategory>();
            services.AddScoped<ICreateLanguage, CreateLanguage>();
            services.AddScoped<IUpdateLanguage, UpdateLanguage>();
            services.AddScoped<IDeleteLanguage, DeleteLanguage>();
            services.AddScoped<ICreateProject, CreateProject>();
            services.AddScoped<IUpdateProject, UpdateProject>();
            services.AddScoped<IDeleteProject, DeleteProject>();
            services.AddScoped<IAddLike, AddLike>();
            services.AddScoped<IRemoveLike, RemoveLike>();
            services.AddScoped<IExistsByIdProject, ExistsByIdProject>();
            
            return services;
        }
    }
}