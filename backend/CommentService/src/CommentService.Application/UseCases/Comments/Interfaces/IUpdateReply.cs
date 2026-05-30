using CommentService.Application.DTOs.Request;

namespace CommentService.Application.UseCases.Comments.Interfaces
{
    public interface IUpdateReply
    {
        Task ExecuteAsync(Guid authenticatedUserId, Guid commentId, CommentRequest commentRequest);
    }
}