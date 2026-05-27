using CommentService.Application.Interfaces;
using CommentService.Application.Services;
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
            return services;
            
        }
    }
}