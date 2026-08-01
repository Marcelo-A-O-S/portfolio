using CommentService.Application.DTOs.Request;
namespace CommentService.Application.UseCases.Comments.Interfaces
{
    public interface IUpdateComment
    {
        Task ExecuteAsync(Guid authenticatedUserId, string role, Guid commentId, CommentRequest commentRequest);
    }
}