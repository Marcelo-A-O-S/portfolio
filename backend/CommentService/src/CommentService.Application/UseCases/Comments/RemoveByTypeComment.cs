using CommentService.Application.Interfaces;
using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Domain.Enums;
using CommentService.Application.Caching.Comment;
using CommentService.Application.Validators.Interfaces;
using CommentService.Domain.Entities;
using CommentService.Application.Exceptions;
namespace CommentService.Application.UseCases.Comments
{
    public class RemoveByTypeComment : IRemoveByTypeComment
    {
        private readonly ICommentServices commentServices;
        private readonly ICommentCacheServices commentCacheServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly ICommentValidationService commentValidationService;
        public RemoveByTypeComment(
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
        public async Task ExecuteAsync(Guid authenticatedUserId, Guid commentId, CommentDeletionType deletionType)
        {
            await this.commentValidationService.ValidateUserExists(authenticatedUserId);
            var comment = await GetComment(commentId);
            if(comment.UserProjection.UserId != authenticatedUserId)
                throw new ValidationException("Você não pode remover este comentário.");
            switch (deletionType)
            {
                case CommentDeletionType.User:
                    comment.DeleteByUser();
                    break;
                case CommentDeletionType.Moderator:
                    comment.DeleteByModerator();
                    break;
                
            }
            await this.commentServices.Update(comment);
            var type = comment.Type.ToString();
            await this.rabbitMQProducer.Publish($"{type}CommentDeleted",
            new
            {
                CommentId = comment.Id,
                TargetId = comment.TargetId,
                Type = comment.Type,
                UserId = comment.UserProjection.UserId
            });
        }
        private async Task<Comment> GetComment(Guid commentId)
        {
            var comment = await commentServices.GetFullDataById(commentId);
            if(comment == null)
                throw new NotFoundException("Comentário não encontrado");
            return comment;
        } 
    }
}