using Microsoft.Extensions.DependencyInjection;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Messaging.Handlers.Interfaces;
using PostService.Infrastructure.Messaging.Handlers;
using PostService.Infrastructure.Repositories;
using PostService.Infrastructure.Workers;

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
            services.AddScoped<IGenerics<Tool>, Generics<Tool>>();
            services.AddScoped<IGenerics<ToolContent>, Generics<ToolContent>>();
            services.AddScoped<IGenerics<Post>, Generics<Post>>();
            services.AddScoped<IGenerics<PostContent>, Generics<PostContent>>();
            services.AddScoped<IGenerics<Language>, Generics<Language>>();
            services.AddScoped<IGenerics<MediaProjection>, Generics<MediaProjection>>();
            services.AddScoped<IGenerics<LikeProjection>, Generics<LikeProjection>>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryContentRepository, CategoryContentRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostContentRepository, PostContentRepository>();
            services.AddScoped<IToolsRepository, ToolsRepository>();
            services.AddScoped<IToolContentRepository, ToolContentRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<IMediaProjectionRepository, MediaProjectionRepository>();
            services.AddScoped<ILikeProjectionRepository, LikeProjectionRepository>();
            
            services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();

            services.AddSingleton<ICommentProjectionHandler, CommentProjectionHandler>();
            services.AddSingleton<ILikeProjectionHandler, LikeProjectionHandler>();
            return services;
        }
    }
}