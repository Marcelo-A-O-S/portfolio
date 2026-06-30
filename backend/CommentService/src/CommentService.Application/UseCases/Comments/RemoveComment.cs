using CommentService.Application.Exceptions;
using CommentService.Application.Interfaces;
using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Application.Validators.Interfaces;
using CommentService.Domain.Entities;
namespace CommentService.Application.UseCases.Comments
{
    public class RemoveComment : IRemoveComment
    {
        private readonly ICommentServices commentServices;
        private readonly ICommentCacheServices commentCacheServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly ICommentValidationService commentValidationService;
        public RemoveComment(
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
        public async Task ExecuteAsync(Guid authenticatedUserId, Guid commentId)
        {
            await this.commentValidationService.ValidateUserExists(authenticatedUserId);
            var comment = await GetComment(commentId);
            if(comment.UserId != authenticatedUserId)
                throw new ForbiddenException("Você não pode editar este comentário.");
            await this.commentServices.DeleteById(commentId);
            var type = comment.Type.ToString();
            await this.commentCacheServices.RemoveCommentCache($"comment:{type}:exists:{commentId}");
            await this.rabbitMQProducer.Publish($"{type}CommentDeleted",
            new
            {
                CommentId = comment.Id,
                TargetId = comment.TargetId,
                Type = comment.Type,
                UserId = comment.UserId
            });
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