using CommentService.Application.Interfaces;
using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Application.Exceptions;
using CommentService.Domain.Entities;
using CommentService.Application.Validations.Interfaces;
namespace CommentService.Application.UseCases.Comments
{
    public class RemoveReply : IRemoveReply
    {
        private readonly ICommentServices commentServices;
        private readonly ICommentCacheServices commentCacheServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly ICommentValidationService commentValidationService;
        public RemoveReply(
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
            if(reply.UserId != authenticatedUserId)
                throw new ValidationException("Você não pode editar esta resposta.");
            if(reply.ParentCommentId != commentId)
                throw new ValidationException("Essa resposta não pertence ao comentário informado.");
            await this.commentServices.DeleteById(reply.Id);
            await this.commentCacheServices.RemoveCommentCache($"comment:exists:{replyId}");
            await this.rabbitMQProducer.Publish("ReplyDelete", new { CommentId = replyId});
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