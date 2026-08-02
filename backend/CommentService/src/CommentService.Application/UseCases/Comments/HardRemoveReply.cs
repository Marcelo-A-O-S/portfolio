using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Application.Interfaces;
using CommentService.Application.Caching.Comment;
using CommentService.Application.Validators.Interfaces;
using CommentService.Domain.Entities;
using CommentService.Application.Exceptions;

namespace CommentService.Application.UseCases.Comments
{
    public class HardRemoveReply : IHardRemoveReply
    {
        private readonly ICommentServices commentServices;
        private readonly ICommentCacheServices commentCacheServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly ICommentValidationService commentValidationService;
        private readonly IUserProjectionServices userProjectionServices;
        private readonly ILikeServices likeServices;
        public HardRemoveReply(
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
        public async Task ExecuteAsync(Guid authenticatedUserId, Guid commentId, Guid replyId)
        {
            await this.commentValidationService.ValidateUserExists(authenticatedUserId);
            var comment = await GetComment(commentId);
            if(comment == null)
                throw new ValidationException("Comentário não pertence a publicação informada.");
            var reply = await GetReply(replyId);
            if(reply.ParentCommentId == null)
                throw new ValidationException("O comentário informado não é uma resposta.");
            if(reply.ParentCommentId != commentId)
                throw new ValidationException("Essa resposta não pertence ao comentário informado.");
            await DeleteReplyTree(reply);
        }
        private async Task<Comment> GetReply(Guid replyId)
        {
            var reply = await commentServices.GetFullDataById(replyId);
            if(reply == null)
                throw new NotFoundException("Resposta não encontrado");
            return reply;
        }
        private async Task<Comment> GetComment(Guid commentId)
        {
            var comment = await commentServices.GetById(commentId);
            if(comment == null)
                throw new NotFoundException("Comentário não encontrado");
            return comment;
        }
        private async Task DeleteReplyTree(Comment comment)
        {
            foreach (var reply in comment.Replies)
            {
                await DeleteReplyTree(reply);
            }
            await this.likeServices.DeleteByCommentId(comment.Id);
            await this.commentServices.DeleteById(comment.Id);
            var type = comment.Type.ToString();
            await this.commentCacheServices.RemoveCommentCache($"comment:{type}:exists:{comment.Id}");
            await this.rabbitMQProducer.Publish($"{type}CommentDeleted",
            new
            {
                CommentId = comment.Id,
                TargetId = comment.TargetId,
                Type = comment.Type,
                UserId = comment.UserProjection.UserId
            });
        }
    }
}