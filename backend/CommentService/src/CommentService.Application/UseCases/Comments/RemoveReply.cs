using CommentService.Application.Interfaces;
using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Application.Exceptions;
using CommentService.Domain.Entities;
using CommentService.Application.Validators.Interfaces;
using CommentService.Application.Caching.Comment;
using CommentService.Domain.Enums;
namespace CommentService.Application.UseCases.Comments
{
    public class RemoveReply : IRemoveReply
    {
        private readonly ICommentServices commentServices;
        private readonly ICommentCacheServices commentCacheServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly ICommentValidationService commentValidationService;
        private readonly IUserProjectionServices userProjectionServices;
        public RemoveReply(
            ICommentServices _commentServices,
            ICommentCacheServices _commentCacheServices,
            IRabbitMQProducer _rabbitMQProducer,
            ICommentValidationService _commentValidationService,
            IUserProjectionServices _userProjectionServices
        )
        {
            this.commentServices = _commentServices;
            this.commentCacheServices = _commentCacheServices;
            this.rabbitMQProducer = _rabbitMQProducer;
            this.commentValidationService = _commentValidationService;
            this.userProjectionServices = _userProjectionServices;
        }
        public async Task ExecuteAsync(Guid authenticatedUserId, string role, Guid commentId, Guid replyId)
        {
            await this.commentValidationService.ValidateUserExists(authenticatedUserId);
            if(!Enum.TryParse<UserRole>(role,true, out var userRole))
                throw new ValidationException("Usuário inválido.");
            var reply = await GetReply(replyId);
            if(userRole == UserRole.Client && reply.UserProjection.UserId != authenticatedUserId)
            {
                throw new ValidationException("Você não pode remover esta resposta.");
            }
            if(reply.ParentCommentId != commentId)
                throw new ValidationException("Essa resposta não pertence ao comentário informado.");
            switch (userRole)
            {
                case UserRole.Administrador:
                    reply.DeleteByAdministrador();
                    break;
                case UserRole.Moderator:
                    reply.DeleteByModerator();
                    break;
                case UserRole.Client:
                    reply.DeleteByUser();
                    break;
            }
            await this.commentServices.Update(reply);
            var type = reply.Type.ToString();
            await this.rabbitMQProducer.Publish($"{type}ReplyDeleted",
            new
            {
                CommentId = reply.Id,
                TargetId = reply.TargetId,
                Type = reply.Type,
                UserId = reply.UserProjection.UserId
            });
        }
        private async Task<Comment> GetReply(Guid replyId)
        {
            var reply = await commentServices.GetFullDataById(replyId);
            if(reply == null)
                throw new NotFoundException("Resposta não encontrado");
            return reply;
        }
    }
}