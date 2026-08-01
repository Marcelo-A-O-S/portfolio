using CommentService.Application.Interfaces;
using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Application.Caching.Comment;
using CommentService.Application.Validators.Interfaces;
using CommentService.Domain.Entities;
using CommentService.Application.Exceptions;

namespace CommentService.Application.UseCases.Comments
{
    public class HardRemoveComment : IHardRemoveComment
    {
        private readonly ICommentServices commentServices;
        private readonly ICommentCacheServices commentCacheServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly ICommentValidationService commentValidationService;
        private readonly IUserProjectionServices userProjectionServices;
        private readonly ILikeServices likeServices;
        public HardRemoveComment(
            ICommentServices _commentServices,
            ICommentCacheServices _commentCacheServices,
            IRabbitMQProducer _rabbitMQProducer,
            ICommentValidationService _commentValidationService,
            IUserProjectionServices _userProjectionServices,
            ILikeServices _likeServices
        )
        {
            this.commentServices = _commentServices;
            this.commentCacheServices = _commentCacheServices;
            this.rabbitMQProducer = _rabbitMQProducer;
            this.commentValidationService = _commentValidationService;
            this.userProjectionServices = _userProjectionServices;
            this.likeServices = _likeServices;
        }
        public async Task ExecuteAsync(Guid authenticatedUserId,Guid commentId)
        {
            await this.commentValidationService.ValidateUserExists(authenticatedUserId);
            var comment = await GetComment(commentId);
            await this.commentServices.DeleteById(commentId);
            await this.likeServices.DeleteByCommentId(commentId);
            var type = comment.Type.ToString();
            await this.commentCacheServices.RemoveCommentCache($"comment:{type}:exists:{commentId}");
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