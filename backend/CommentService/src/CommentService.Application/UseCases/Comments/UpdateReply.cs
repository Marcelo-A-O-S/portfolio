using CommentService.Application.DTOs.Request;
using CommentService.Application.Exceptions;
using CommentService.Application.Interfaces;
using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Application.Validations;
using CommentService.Application.Validations.Interfaces;
using CommentService.Domain.Entities;
namespace CommentService.Application.UseCases.Comments
{
    public class UpdateReply : IUpdateReply
    {
        private readonly ICommentCacheServices commentCacheServices;
        private readonly ICommentServices commentServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly ICommentValidationService commentValidationService;
        public UpdateReply(
            ICommentCacheServices _commentCacheServices,
            ICommentServices _commentServices,
            IRabbitMQProducer _rabbitMQProducer,
            ICommentValidationService _commentValidationService
        )
        {
            this.commentCacheServices = _commentCacheServices;
            this.commentServices = _commentServices;
            this.rabbitMQProducer = _rabbitMQProducer;
            this.commentValidationService = _commentValidationService;
        }
        public async Task ExecuteAsync(Guid authenticatedUserId, Guid commentId, Guid replyId, CommentRequest commentRequest)
        {
            ValidateRequest(commentRequest);
            await this.commentValidationService.ValidateUserExists(authenticatedUserId);
            await this.commentValidationService.ValidateTargetExists(commentRequest.TargetId, commentRequest.Type);
            var comment = await GetComment(commentId);
            if(comment.TargetId != commentRequest.TargetId)
                throw new ValidationException("Comentário não pertence a publicação informada.");
            var reply = await GetReply(replyId);
            if(reply.UserId != authenticatedUserId)
                throw new ValidationException("Você não pode editar esta resposta.");
            if(reply.ParentCommentId == null)
                throw new ValidationException("O comentário informado não é uma resposta.");
            if(reply.ParentCommentId != commentId)
                throw new ValidationException("Essa resposta não pertence ao comentário informado.");
            reply.Update(commentRequest.Content);
            await this.commentServices.Update(reply);
            var type = reply.Type.ToString();
            await this.commentCacheServices.AddCommentCache($"comment:{type}:exists:{reply.Id}", reply.Id);
            
        }
        private static void ValidateRequest(CommentRequest request)
        {
            if(request.Id == null)
                throw new ValidationException("Identificador da resposta não informado.");
            var validationError = ValidationHelper.Validate(request);
            if(validationError.Count > 0)
            {
                var errors = string.Join(", ",validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {errors}");
            }
        }
        private async Task<Comment> GetComment(Guid commentId)
        {
            var comment = await commentServices.GetById(commentId);
            if(comment == null)
                throw new NotFoundException("Comentário não encontrado");
            return comment;
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