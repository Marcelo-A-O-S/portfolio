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
using PostService.Application.UseCases.InternalProject.Interfaces;
using PostService.Application.UseCases.InternalProject;
using PostService.Application.Caching.User;
using PostService.Application.Caching.Post;
using PostService.Application.UseCases.InternalTool.Interfaces;
using PostService.Application.UseCases.InternalTool;
using PostService.Application.Caching.Tools;
using PostService.Application.Validators.Interfaces;
using PostService.Application.Validators;
using PostService.Application.UseCases.LinkTypes.Interfaces;
using PostService.Application.UseCases.LinkTypes;
using PostService.Application.UseCases.Links.Interfaces;
using PostService.Application.UseCases.Links;
using PostService.Domain.Interfaces;
using PostService.Application.Caching.Language;
namespace PostService.Application.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjection(
            this IServiceCollection services
        )
        {
            services.AddScoped<IPostServices, PostServices>();
            services.AddScoped<IPostContentServices, PostContentServices>();
            services.AddScoped<ICategoryServices, CategoryServices>();
            services.AddScoped<ICategoryContentServices, CategoryContentServices>();
            services.AddScoped<IToolsServices, ToolsServices>();
            services.AddScoped<IToolContentServices, ToolContentServices>();
            services.AddScoped<ILanguageServices, LanguageServices>();
            services.AddScoped<IMediaProjectionServices, MediaProjectionServices>();
            services.AddScoped<ILikeProjectionServices, LikeProjectionServices>();
            services.AddScoped<IAuthorServices, AuthorServices>();
            services.AddScoped<ILinkServices, LinkServices>();
            services.AddScoped<ILinkTypeServices, LinkTypeServices>();
            services.AddScoped<ILinkDescriptionServices, LinkDescriptionServices>();
            services.AddScoped<IContributorServices, ContributorServices>();

            services.AddScoped<IUserCacheServices, UserCacheServices>();
            services.AddScoped<IPostCacheServices, PostCacheServices>();
            services.AddScoped<IToolCacheServices, ToolCacheServices>();
            services.AddScoped<ILanguageCacheServices, LanguageCacheServices>();

            services.AddScoped<IToolValidationServices, ToolValidationServices>();
            services.AddScoped<IValidationServices, ValidationServices>();

            services.AddScoped<ICreateLanguage, CreateLanguage>();
            services.AddScoped<IUpdateLanguage, UpdateLanguage>();
            services.AddScoped<IDeleteLanguage, DeleteLanguage>();
            services.AddScoped<ICreateLinkType, CreateLinkType>();
            services.AddScoped<IUpdateLinkType, UpdateLinkType>();
            services.AddScoped<IDeleteLinkType, DeleteLinkType>();
            services.AddScoped<ICreateLink, CreateLink>();
            services.AddScoped<IUpdateLink, UpdateLink>();
            services.AddScoped<IDeleteLink, DeleteLink>();
            services.AddScoped<ICreateCategory, CreateCategory>();
            services.AddScoped<IUpdateCategory, UpdateCategory>();
            services.AddScoped<IDeleteCategory, DeleteCategory>();
            services.AddScoped<ICreateTool, CreateTool>();
            services.AddScoped<IUpdateTool, UpdateTool>();
            services.AddScoped<IDeleteTool, DeleteTool>();
            services.AddScoped<IAddLinkTool, AddLinkTool>();
            services.AddScoped<IUpdateLinkTool, UpdateLinkTool>();
            services.AddScoped<IDeleteLinkTool, DeleteLinkTool>();
            services.AddScoped<IExistsByIdTool, ExistsByIdTool>();
            services.AddScoped<ICreateProject, CreateProject>();
            services.AddScoped<IUpdateProject, UpdateProject>();
            services.AddScoped<IDeleteProject, DeleteProject>();
            services.AddScoped<IAddLinkProject, AddLinkProject>();
            services.AddScoped<IUpdateLinkProject, UpdateLinkProject>();
            services.AddScoped<IDeleteLinkProject, DeleteLinkProject>();
            services.AddScoped<IExistsByIdProject, ExistsByIdProject>();
            
            return services;
        }
    }
}