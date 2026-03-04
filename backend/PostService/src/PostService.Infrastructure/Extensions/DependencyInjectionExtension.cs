using Microsoft.Extensions.DependencyInjection;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Repositories;

namespace PostService.Infrastructure.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjection(
            this IServiceCollection services
        )
        {
            services.AddScoped<IGenerics<Category>,Generics<Category>>();
            services.AddScoped<IGenerics<Like>, Generics<Like>>();
            services.AddScoped<IGenerics<Section>, Generics<Section>>();
            services.AddScoped<IGenerics<Tool>, Generics<Tool>>();
            services.AddScoped<IGenerics<Post>, Generics<Post>>();
            services.AddScoped<IGenerics<PostTool>, Generics<PostTool>>();
            services.AddScoped<IGenerics<PostCategory>, Generics<PostCategory>>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddScoped<IPostCategoryRepository, PostCategoryRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostToolsRepository, PostToolsRepository>();
            services.AddScoped<ISectionRepository, SectionRepository>();
            services.AddScoped<IToolsRepository, ToolsRepository>();
            return services;
        }
    }
}