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
            services.AddScoped<IGenerics<Post>, Generics<Post>>();
            services.AddScoped<IGenerics<Category>,Generics<Category>>();
            services.AddScoped<IGenerics<PostCategory>, Generics<PostCategory>>();
            services.AddScoped<IGenerics<Like>, Generics<Like>>();
            services.AddScoped<IGenerics<Section>, Generics<Section>>();

            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IPostCategoryRepository, PostCategoryRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddScoped<ISectionRepository, SectionRepository>();
            return services;
        }
    }
}