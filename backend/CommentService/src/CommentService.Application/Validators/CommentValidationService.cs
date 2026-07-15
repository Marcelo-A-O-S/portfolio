using CommentService.Application.Interfaces;
using CommentService.Application.Exceptions;
using CommentService.Application.Validators.Interfaces;
using CommentService.Domain.Enums;
using CommentService.Application.Caching.Users;
using CommentService.Application.Caching.Posts;
using CommentService.Application.Caching.Comment;
using CommentService.Application.Caching.Tools;
using CommentService.Application.Constants;

namespace CommentService.Application.Validators
{
    public class CommentValidationService : ICommentValidationService
    {
        private readonly IUserCacheServices userCacheServices;
        private readonly IPostCacheServices postCacheServices;
        private readonly IPostServicesClient postServicesClient;
        private readonly IUserServicesClient userServicesClient;
        private readonly ICommentServices commentServices;
        private readonly ICommentCacheServices commentCacheServices;
        private readonly IToolCacheServices toolCacheServices;
        private readonly IToolServicesClient toolServicesClient;
        public CommentValidationService(
            IUserCacheServices _userCacheServices,
            IPostCacheServices _postCacheServices,
            IPostServicesClient _postServicesClient,
            IUserServicesClient _userServicesClient,
            ICommentServices _commentServices,
            ICommentCacheServices _commentCacheServices,
            IToolCacheServices _toolCacheServices,
            IToolServicesClient _toolServicesClient
        )
        {
             this.userCacheServices = _userCacheServices;
            this.postCacheServices = _postCacheServices;
            this.postServicesClient = _postServicesClient;
            this.userServicesClient = _userServicesClient;
            this.commentServices = _commentServices;
            this.commentCacheServices = _commentCacheServices;
            this.toolCacheServices = _toolCacheServices;
            this.toolServicesClient = _toolServicesClient;
        }
        public async Task ValidateCommentExists(Guid commentId)
        {
            var commentCache = await this.commentCacheServices.GetCommentCache(CacheKeys.CommentExists(commentId));
            if (commentCache == null)
            {
                var exists = await this.commentServices.Exists(commentId);
                if (!exists)
                    throw new NotFoundException("Commentário não encontrado");
                await this.commentCacheServices.AddCommentCache(CacheKeys.CommentExists(commentId), commentId);
            }
        }

        public async Task ValidatePostExists(Guid postId)
        {
            var postCache = await this.postCacheServices.GetPostCache(CacheKeys.PostExists(postId));
            if(postCache == null)
            {
                var exists = await this.postServicesClient.PostExistsAsync(postId);
                if (!exists)
                    throw new NotFoundException("Projeto não encontrado");
                await this.postCacheServices.AddPostCache(CacheKeys.PostExists(postId), postId);
            }
        }

        public async Task ValidateProviderExists(Guid userId, string providerId)
        {
            var providerCache = await this.userCacheServices.GetProviderCache(CacheKeys.ProviderExists(providerId));
            if(providerCache == null)
            {
                var exists = await this.userServicesClient.ProviderExistsAsync(userId, providerId);
                if (!exists)
                    throw new NotFoundException("Provider não encontrado");
                await this.userCacheServices.AddProviderCache(CacheKeys.ProviderExists(providerId), providerId);
            }
        }

        public async Task ValidateReply(Guid replyId)
        {
            var replyCache = await this.commentCacheServices.GetCommentCache(CacheKeys.ReplyExists(replyId));
            if(replyCache == null)
            {
                var exists = await this.commentServices.Exists(replyId);
                if(!exists)
                    throw new NotFoundException("Resposta não encontrada.");
                await this.commentCacheServices.AddCommentCache(CacheKeys.ReplyExists(replyId), replyId);
            }
        }

        public async Task ValidateTargetExists(Guid targetId, CommentType type)
        {
            switch (type)
            {
                case CommentType.Post:
                    await ValidatePostExists(targetId);
                    break;
                case CommentType.Tool:
                    await ValidateToolExists(targetId);
                    break;
                default:
                    throw new ValidationException("Tipo de comentário inválido.");
            }
        }
        public async Task ValidateToolExists(Guid toolId)
        {
            var toolCache = await this.toolCacheServices.GetToolCache(CacheKeys.ToolExists(toolId));
            if(toolCache == null)
            {
                var exists = await this.toolServicesClient.ToolExistsAsync(toolId);
                if(!exists)
                    throw new NotFoundException("Ferramenta não encontrada");
                await this.toolCacheServices.AddToolCache(CacheKeys.ToolExists(toolId), toolId);
            }
        }

        public async Task ValidateUserExists(Guid userId)
        {
            var userCache = await this.userCacheServices.GetUserCache(CacheKeys.UserExists(userId));
            if (userCache == null)
            {
                var exists = await this.userServicesClient.UserExistsAsync(userId);
                if (!exists)
                    throw new NotFoundException("Usuário não encontrado");
                await this.userCacheServices.AddUserCache(CacheKeys.UserExists(userId), userId);
            }
        }
    }
}