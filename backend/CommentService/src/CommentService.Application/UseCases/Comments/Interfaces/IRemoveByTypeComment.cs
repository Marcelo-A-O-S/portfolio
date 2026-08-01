using CommentService.Domain.Enums;
namespace CommentService.Application.UseCases.Comments.Interfaces
{
    public interface IRemoveByTypeComment
    {
        Task ExecuteAsync(Guid authenticatedUserId, Guid commentId, CommentDeletionType deletionType);
    }
}