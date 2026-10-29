using System.Text.Json;
using CommentService.Application.Caching.Comment;
using CommentService.Application.Constants;
using CommentService.Application.Exceptions;
using CommentService.Application.Interfaces;
using CommentService.Application.UseCases.Comments.Interfaces;
using CommentService.Application.Validators.Interfaces;
using CommentService.Domain.Entities;
using CommentService.Domain.Enums;
namespace CommentService.Application.UseCases.Comments
{
    public class RemoveComment : IRemoveComment
    {
        private readonly ICommentServices commentServices;
        private readonly ICommentCacheServices commentCacheServices;
        private readonly ICommentValidationService commentValidationService;
        private readonly IOutboxMessageServices outboxMessageServices;
        private readonly IUnitOfWork unitOfWork;
        public RemoveComment(
            ICommentServices _commentServices,
            ICommentCacheServices _commentCacheServices,
            ICommentValidationService _commentValidationService,
            IOutboxMessageServices _outboxMessageServices,
            IUnitOfWork _unitOfWork
        )
        {
            this.commentServices = _commentServices;
            this.commentCacheServices = _commentCacheServices;
            this.commentValidationService = _commentValidationService;
            this.outboxMessageServices = _outboxMessageServices;
            this.unitOfWork = _unitOfWork;
        }

        public async Task ExecuteAsync(Guid authenticatedUserId, string role, Guid commentId)
        {
            await this.commentValidationService.ValidateUserExists(authenticatedUserId);
            if (!Enum.TryParse<UserRole>(role, true, out var userRole))
                throw new ValidationException("Usuário inválido.");
            var comment = await GetComment(commentId);
            if (userRole == UserRole.Client && comment.UserProjection.UserId != authenticatedUserId)
            {
                throw new ValidationException("Você não pode remover este comentário.");
            }
            await this.unitOfWork.BeginAsync();
            var type = comment.Type.ToString();
            try
            {
                switch (userRole)
                {
                    case UserRole.Administrador:
                        comment.DeleteByAdministrador();
                        break;
                    case UserRole.Moderator:
                        comment.DeleteByModerator();
                        break;
                    case UserRole.Client:
                        comment.DeleteByUser();
                        break;
                    default:
                        throw new ValidationException("Usuário inválido");
                }
                await this.commentServices.Update(comment);
                type = comment.Type.ToString();
                var outboxMessage = new OutboxMessage(
                    $"{type}CommentDeleted",
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
        private async Task<Comment> GetComment(Guid commentId)
        {
            var comment = await commentServices.GetFullDataById(commentId);
            if (comment == null)
                throw new NotFoundException("Comentário não encontrado");
            return comment;
        }
    }
}