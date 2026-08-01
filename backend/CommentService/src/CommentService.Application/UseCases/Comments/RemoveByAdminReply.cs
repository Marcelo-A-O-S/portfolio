using CommentService.Application.Caching.Comment;
using CommentService.Application.Exceptions;
using CommentService.Application.Interfaces;
using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Application.Validators.Interfaces;
using CommentService.Domain.Entities;
namespace CommentService.Application.UseCases.Comments
{
    public class RemoveByAdminReply : IRemoveByAdminReply
    {
        private readonly ICommentServices commentServices;
        private readonly ICommentCacheServices commentCacheServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly ICommentValidationService commentValidationService;
        public RemoveByAdminReply(
            ICommentServices _commentServices,
            ICommentCacheServices _commentCacheServices,
            IRabbitMQProducer _rabbitMQProducer,
            ICommentValidationService _commentValidationService
        )
        {
            this.commentServices = _commentServices;
            this.commentCacheServices = _commentCacheServices;
            this.rabbitMQProducer = _rabbitMQProducer;
            this.commentValidationService = _commentValidationService;
        }
        public async Task ExecuteAsync(Guid authenticatedUserId, Guid commentId, Guid replyId)
        {
            await this.commentValidationService.ValidateUserExists(authenticatedUserId);
            var reply = await GetReply(replyId);
            if(reply.ParentCommentId != commentId)
                throw new ValidationException("Essa resposta não pertence ao comentário informado.");
            reply.DeleteByAdministrador();
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
            var reply = await commentServices.GetById(replyId);
            if(reply == null)
                throw new NotFoundException("Resposta não encontrado");
            return reply;
        }
    }
}