using CommentService.Application.Interfaces;
using CommentService.Application.Services;
using CommentService.Application.UseCases.Comments;
using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Application.UseCases.Likes;
using CommentService.Application.UseCases.Likes.Interfaces;
using CommentService.Application.Validators.Interfaces;
using CommentService.Application.Validators;
using Microsoft.Extensions.DependencyInjection;
using CommentService.Application.Caching.Users;
using CommentService.Application.Caching.Posts;
using CommentService.Application.Caching.Like;
using CommentService.Application.Caching.Comment;
using CommentService.Application.Caching.Tools;
namespace CommentService.Application.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjectionExtension(
            this IServiceCollection services
        )
        {
            services.AddScoped<ICommentServices, CommentServices>();
            services.AddScoped<ILikeServices, LikeServices>();
            services.AddScoped<ICommentProjectionServices, CommentProjectionServices>();
            services.AddScoped<IUserProjectionServices, UserProjectionServices>();

            services.AddScoped<IUserCacheServices, UserCacheServices>();
            services.AddScoped<IPostCacheServices, PostCacheServices>();
            services.AddScoped<ILikeCacheServices, LikeCacheServices>();
            services.AddScoped<ICommentCacheServices, CommentCacheServices>();
            services.AddScoped<IToolCacheServices, ToolCacheServices>();

            services.AddScoped<ICommentValidationService, CommentValidationService>();
            services.AddScoped<ILikeValidationService, LikeValidationService>();

            services.AddScoped<IAddComment, AddComment>();
            services.AddScoped<IRemoveComment, RemoveComment>();
            services.AddScoped<IUpdateComment, UpdateComment>();
            services.AddScoped<IRemoveByUserComment, RemoveByUserComment>();
            services.AddScoped<IRemoveByModeratorComment, RemoveByModeratorComment>();
            services.AddScoped<IAddReply, AddReply>();
            services.AddScoped<IRemoveReply, RemoveReply>();
            services.AddScoped<IUpdateReply, UpdateReply>();
            services.AddScoped<IRemoveByUserReply, RemoveByUserReply>();
            services.AddScoped<IRemoveByModeratorReply, RemoveByModeratorReply>();
            services.AddScoped<IAddLike, AddLike>();
            services.AddScoped<IRemoveLike, RemoveLike>();
            
            return services;

        }
    }
}