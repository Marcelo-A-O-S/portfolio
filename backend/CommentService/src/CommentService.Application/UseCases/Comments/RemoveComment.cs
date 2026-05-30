using CommentService.Application.Exceptions;
using CommentService.Application.Interfaces;
using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Domain.Entities;
namespace CommentService.Application.UseCases.Comments
{
    public class RemoveComment : IRemoveComment
    {
        private readonly ICommentServices commentServices;
        private readonly ICommentCacheServices commentCacheServices;
        private readonly IUserCacheServices userCacheServices;
        private readonly IUserServicesClient userServicesClient;
        public RemoveComment(
            ICommentServices _commentServices,
            ICommentCacheServices _commentCacheServices,
            IUserCacheServices _userCacheServices,
            IUserServicesClient _userServicesClient
        )
        {
            this.commentServices = _commentServices;
            this.commentCacheServices = _commentCacheServices;
            this.userCacheServices = _userCacheServices;
            this.userServicesClient = _userServicesClient;
        }
        public async Task ExecuteAsync(Guid authenticatedUserId, Guid commentId)
        {
            await ValidateComment(commentId);
            await ValidateUserExists(authenticatedUserId);
            var comment = await GetComment(commentId);
            if(comment.UserId != authenticatedUserId)
                throw new ForbiddenException("Você não pode editar este comentário.");
            await this.commentServices.DeleteById(commentId);
            await this.commentCacheServices.RemoveCommentCache($"comment:exists:{commentId}");
        }
        private async Task ValidateComment(Guid commentId)
        {
            var commentCache = await this.commentCacheServices.GetCommentCache($"comment:exists:{commentId}");
            if(commentCache == null)
            {
                var comment = await this.commentServices.Exists(commentId);
                if(!comment)
                    throw new NotFoundException("Comentário não encontrado");
            }
        }
        private async Task ValidateUserExists(Guid userId)
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
        private async Task<Comment> GetComment(Guid commentId)
        {
            return await this.commentServices.GetById(commentId);
        } 
    }
}