using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Application.Interfaces;
using CommentService.Application.Caching.Comment;
using CommentService.Application.Validators.Interfaces;
using CommentService.Domain.Entities;
using CommentService.Application.Exceptions;
using System.Text.Json;
using CommentService.Application.Constants;

namespace CommentService.Application.UseCases.Comments
{
    public class HardRemoveReply : IHardRemoveReply
    {
        private readonly ICommentServices commentServices;
        private readonly ICommentCacheServices commentCacheServices;
        private readonly ICommentValidationService commentValidationService;
        private readonly ILikeServices likeServices;
        private readonly IOutboxMessageServices outboxMessageServices;
        private readonly IUnitOfWork unitOfWork;
        public HardRemoveReply(
            ICommentServices _commentServices,
            ICommentCacheServices _commentCacheServices,
            ICommentValidationService _commentValidationService,
            ILikeServices _likeServices,
            IOutboxMessageServices _outboxMessageServices,
            IUnitOfWork _unitOfWork
        )
        {
            this.commentServices = _commentServices;
            this.commentCacheServices = _commentCacheServices;
            this.commentValidationService = _commentValidationService;
            this.likeServices = _likeServices;
            this.outboxMessageServices = _outboxMessageServices;
            this.unitOfWork = _unitOfWork;
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
            foreach (var reply in comment.Replies.ToList())
            {
                await DeleteReplyTree(reply);
            }
            await this.unitOfWork.BeginAsync();
            var type = comment.Type.ToString();
            try
            {
                await this.likeServices.DeleteByCommentId(comment.Id);
                await this.commentServices.DeleteById(comment.Id);
                var outboxMessage = new OutboxMessage(
                    $"{type}ReplyDeleted",
                    JsonSerializer.Serialize(new
                    {
                        CommentId = comment.Id,
                        TargetId = comment.TargetId,
                        Type = comment.Type,
                        UserId = comment.UserProjection.UserId
                    })
                );
                await this.outboxMessageServices.Save(outboxMessage);
                await this.unitOfWork.CommitAsync();
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
            await this.commentCacheServices.RemoveCommentCache(CacheKeys.CommentTypeExists(type, comment.Id));
        }
    }
}