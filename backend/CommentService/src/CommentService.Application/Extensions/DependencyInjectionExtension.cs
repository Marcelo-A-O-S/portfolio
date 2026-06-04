using CommentService.Application.Interfaces;
using CommentService.Application.Services;
using CommentService.Application.UseCases.Comments;
using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Application.UseCases.Likes;
using CommentService.Application.UseCases.Likes.Interfaces;
using CommentService.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
namespace CommentService.Application.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjectionExtension(
            this IServiceCollection services
        )
        {
            services.AddScoped<ICommentServices, CommentServices>();
            services.AddScoped<IAnswerServices, AnswerServices>();
            services.AddScoped<ILikeServices, LikeServices>();

            services.AddScoped<IUserCacheServices, UserCacheServices>();
            services.AddScoped<IPostCacheServices, PostCacheServices>();
            services.AddScoped<ILikeCacheServices, LikeCacheServices>();
            services.AddScoped<ICommentCacheServices, CommentCacheServices>();

            services.AddScoped<IAddComment, AddComment>();
            services.AddScoped<IRemoveComment, RemoveComment>();
            services.AddScoped<IUpdateComment, UpdateComment>();
            services.AddScoped<IAddReply, AddReply>();
            services.AddScoped<IRemoveReply, RemoveReply>();
            services.AddScoped<IUpdateReply, UpdateReply>();
            services.AddScoped<IAddLike, AddLike>();
            services.AddScoped<IRemoveLike, RemoveLike>();
            return services;

        }
    }
}