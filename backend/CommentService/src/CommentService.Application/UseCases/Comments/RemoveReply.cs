using CommentService.Application.Interfaces;
using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Application.Exceptions;
using CommentService.Domain.Entities;

namespace CommentService.Application.UseCases.Comments
{
    public class RemoveReply : IRemoveReply
    {
        private readonly ICommentServices commentServices;
        private readonly ICommentCacheServices commentCacheServices;
        private readonly IUserCacheServices userCacheServices;
        private readonly IUserServicesClient userServicesClient;
        public RemoveReply(
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
        public async Task ExecuteAsync(Guid authenticatedUserId, Guid commentId, Guid replyId)
        {
            await ValidateReply(replyId);
            await ValidateUserExists(authenticatedUserId);
            var reply = await GetReply(replyId);
            if(reply.UserId != authenticatedUserId)
                throw new ValidationException("Você não pode editar esta resposta.");
            if(reply.ParentCommentId != commentId)
                throw new ValidationException("Essa resposta não pertence ao comentário informado.");
            await this.commentServices.DeleteById(reply.Id);
            await this.commentCacheServices.RemoveCommentCache($"comment:reply:exists:{replyId}");
        }
        private async Task ValidateReply(Guid replyId)
        {
            var replyCache = await this.commentCacheServices.GetCommentCache($"comment:reply:exists:{replyId}");
            if(replyCache == null)
            {
                var exists = await this.commentServices.Exists(replyId);
                if(!exists)
                    throw new NotFoundException("Resposta não encontrada.");
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
        private async Task<Comment> GetReply(Guid replyId)
        {
            var reply = await commentServices.GetById(replyId);
            if(reply == null)
                throw new NotFoundException("Resposta não encontrado");
            return reply;
        }
    }
}