using CommentService.Application.DTOs.Request;
using CommentService.Application.Exceptions;
using CommentService.Application.Interfaces;
using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Application.Validations;
using CommentService.Application.Validations.Interfaces;
using CommentService.Domain.Entities;
namespace CommentService.Application.UseCases.Comments
{
    public class UpdateComment : IUpdateComment
    {
        private readonly ICommentCacheServices commentCacheServices;
        private readonly ICommentServices commentServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly ICommentValidationService commentValidationService;
        public UpdateComment(
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
        public async Task ExecuteAsync(Guid authenticatedUserId, Guid commentId, CommentRequest commentRequest)
        {
            ValidateRequest(commentRequest);
            await this.commentValidationService.ValidateUserExists(authenticatedUserId);
            await this.commentValidationService.ValidateTargetExists(commentRequest.TargetId, commentRequest.Type);
            await this.commentValidationService.ValidateCommentExists(commentId);
            var comment = await GetComment(commentId);
            if(comment.UserId != authenticatedUserId)
                throw new ForbiddenException("Você não pode editar este comentário.");
            if(comment.TargetId != commentRequest.TargetId)
                throw new ValidationException("Comentário não pertence ao post informado.");
            if(comment.Type != commentRequest.Type)
                throw new ValidationException("Tipo de comentário inválido.");
            comment.Update(commentRequest.Content);
            await this.commentServices.Update(comment);
            var type = comment.Type.ToString();
            await this.commentCacheServices.AddCommentCache($"comment:{type}:exists:{comment.Id}", comment.Id);
            
        }
        private static void ValidateRequest(CommentRequest commentRequest)
        {
            var validationError = ValidationHelper.Validate(commentRequest);
            if (validationError.Count > 0)
            {
                var errors = string.Join(", ", validationError.Select(e => e.ErrorMessage));
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
    }
}