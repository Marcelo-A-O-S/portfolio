using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Application.Interfaces;
using CommentService.Application.Validators.Interfaces;
using CommentService.Application.Exceptions;
using CommentService.Domain.Entities;
namespace CommentService.Application.UseCases.Comments
{
    public class RemoveByUserComment : IRemoveByUserComment
    {
        private readonly ICommentServices commentServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly ICommentValidationService commentValidationService;
        public RemoveByUserComment(
            ICommentServices _commentServices,
            IRabbitMQProducer _rabbitMQProducer,
            ICommentValidationService _commentValidationService
        )
        {
            this.commentServices = _commentServices;
            this.rabbitMQProducer = _rabbitMQProducer;
            this.commentValidationService = _commentValidationService;
        }
        public async Task ExecuteAsync(Guid authenticatedUserId, Guid commentId)
        {
            await this.commentValidationService.ValidateUserExists(authenticatedUserId);
            var comment = await GetComment(commentId);
            if(comment.UserProjection.UserId != authenticatedUserId)
                throw new ValidationException("Você não pode remover este comentário.");
            comment.DeleteByUser();
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