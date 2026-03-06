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
            services.AddScoped<IGenerics<CategoryContent>, Generics<CategoryContent>>();
            services.AddScoped<IGenerics<Like>, Generics<Like>>();
            services.AddScoped<IGenerics<Tool>, Generics<Tool>>();
            services.AddScoped<IGenerics<ToolContent>, Generics<ToolContent>>();
            services.AddScoped<IGenerics<Post>, Generics<Post>>();
            services.AddScoped<IGenerics<PostContent>, Generics<PostContent>>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryContentRepository, CategoryContentRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostContentRepository, PostContentRepository>();
            services.AddScoped<IToolsRepository, ToolsRepository>();
            services.AddScoped<IToolContentRepository, ToolContentRepository>();
            return services;
        }
    }
}