using Microsoft.Extensions.DependencyInjection;
using CommentService.Domain.Interfaces;
using CommentService.Domain.Entities;
using CommentService.Infrastructure.Repositories;

namespace CommentService.Infrastructure.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjectionExtension(
            this IServiceCollection services
        ){
            services.AddScoped<IGenerics<Comment>, Generics<Comment>>();
            services.AddScoped<IGenerics<Like>, Generics<Like>>();

            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            return services;
        } 
    }
}