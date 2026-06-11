using CommentService.Application.DTOs.Request;
using CommentService.Application.Exceptions;
using CommentService.Application.Interfaces;
using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Application.Validations;
using CommentService.Application.Validations.Interfaces;
using CommentService.Domain.Entities;
namespace CommentService.Application.UseCases.Comments
{
    public class AddComment : IAddComment
    {
        private readonly ICommentServices commentServices;
        private readonly ICommentCacheServices commentCacheServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly ICommentValidationService commentValidationService;
        public AddComment(
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
        public async Task ExecuteAsync(Guid authenticatedUserId, CommentRequest commentRequest)
        {
            ValidateRequest(commentRequest);
            await this.commentValidationService.ValidateUserExists(authenticatedUserId);
            await this.commentValidationService.ValidateTargetExists(commentRequest.TargetId, commentRequest.Type);
            var comment = new Comment(authenticatedUserId, commentRequest.TargetId, commentRequest.Type, commentRequest.Content);
            await this.commentServices.Save(comment);
            var type = comment.Type.ToString();
            await this.commentCacheServices.AddCommentCache($"comment:{type}:exists:{comment.Id}", comment.Id);
            await this.rabbitMQProducer.Publish($"{type}CommentCreated",
            new
            {
                CommentId = comment.Id,
                TargetId = comment.TargetId,
                Type = comment.Type,
                UserId = comment.UserId
            });
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
    }
}