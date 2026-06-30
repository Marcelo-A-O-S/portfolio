using CommentService.Domain.Enums;
using CommentService.Application.Exceptions;
using CommentService.Application.Interfaces;
using CommentService.Application.Validators.Interfaces;

namespace CommentService.Application.Validators
{
    public class LikeValidationService : ILikeValidationService
    {
        private readonly IUserCacheServices userCacheServices;
        private readonly IPostCacheServices postCacheServices;
        private readonly IPostServicesClient postServicesClient;
        private readonly IUserServicesClient userServicesClient;
        private readonly ICommentServices commentServices;
        private readonly ICommentCacheServices commentCacheServices;
        private readonly IToolCacheServices toolCacheServices;
        private readonly IToolServicesClient toolServicesClient;
        public LikeValidationService(
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
            var commentCache = await this.commentCacheServices.GetCommentCache($"comment:exists:{commentId}");
            if (commentCache == null)
            {
                var exists = await this.commentServices.Exists(commentId);
                if (!exists)
                    throw new NotFoundException("Commentário não encontrado");
                await this.commentCacheServices.AddCommentCache($"comment:exists:{commentId}", commentId);
            }
        }

        public async Task ValidatePostExists(Guid postId)
        {
            var postCache = await this.postCacheServices.GetPostCache($"post:exists:{postId}");
            if(postCache == null)
            {
                var exists = await this.postServicesClient.PostExistsAsync(postId);
                if (!exists)
                    throw new NotFoundException("Projeto não encontrado");
                await this.postCacheServices.AddPostCache($"post:exists:{postId}", postId);
            }
        }

        public async Task ValidateTargetExists(Guid targetId, LikeType type)
        {
            switch (type)
            {
                case LikeType.Post:
                    await ValidatePostExists(targetId);
                    break;
                case LikeType.Tool:
                    await ValidateToolExists(targetId);
                    break;
                case LikeType.Comment:
                    await ValidateCommentExists(targetId);
                    break;
                default:
                    throw new ValidationException("Tipo de curtida inválida.");
            }
        }

        public async Task ValidateToolExists(Guid toolId)
        {
            var toolCache = await this.toolCacheServices.GetToolCache($"tool:exists:{toolId}");
            if(toolCache == null)
            {
                var exists = await this.toolServicesClient.ToolExistsAsync(toolId);
                if(!exists)
                    throw new NotFoundException("Ferramenta não encontrada");
                await this.toolCacheServices.AddToolCache($"tool:exists:{toolId}", toolId);
            }
        }

        public async Task ValidateUserExists(Guid userId)
        {
            var userCache = await this.userCacheServices.GetUserCache($"user:exists:{userId}");
            if (userCache == null)
            {
                var exists = await this.userServicesClient.UserExistsAsync(userId);
                if (!exists)
                    throw new NotFoundException("Usuário não encontrado");
                await this.userCacheServices.AddUserCache($"user:exists:{userId}", userId);
            }
        }
    }
}