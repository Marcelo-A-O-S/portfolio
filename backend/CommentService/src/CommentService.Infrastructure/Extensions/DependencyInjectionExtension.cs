using Microsoft.Extensions.DependencyInjection;
using CommentService.Domain.Interfaces;
using CommentService.Domain.Entities;
using CommentService.Infrastructure.Repositories;
using CommentService.Application.Interfaces;
using CommentService.Infrastructure.Workers;
using CommentService.Infrastructure.Messaging.Handlers.Interfaces;
using CommentService.Infrastructure.Messaging.Handlers;

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

            services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();

            services.AddSingleton<IPostEventHandler, PostEventHandler>();
            services.AddSingleton<IUserEventHandler, UserEventHandler>();
            services.AddSingleton<ICommentEventHandler, CommentEventHandler>();
            return services;
        } 
    }
}