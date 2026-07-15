using CommentService.Application.Interfaces;
using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Application.Exceptions;
using CommentService.Domain.Entities;
using CommentService.Application.Validators.Interfaces;
using CommentService.Application.Caching.Comment;
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
        public async Task ExecuteAsync(Guid authenticatedUserId, Guid commentId, Guid replyId)
        {
            await this.commentValidationService.ValidateUserExists(authenticatedUserId);
            var reply = await GetReply(replyId);
            if(reply.UserProjection.UserId != authenticatedUserId)
                throw new ValidationException("Você não pode editar esta resposta.");
            if(reply.ParentCommentId != commentId)
                throw new ValidationException("Essa resposta não pertence ao comentário informado.");
            var userProjection = await this.userProjectionServices.GetById(reply.UserProjectionId);
            await this.commentServices.DeleteById(reply.Id);
            await this.userProjectionServices.DeleteById(userProjection.Id);
            var type = reply.Type.ToString();
            await this.commentCacheServices.RemoveCommentCache($"comment:{type}:exists:{replyId}");
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
            var reply = await commentServices.GetById(replyId);
            if(reply == null)
                throw new NotFoundException("Resposta não encontrado");
            return reply;
        }
    }
}