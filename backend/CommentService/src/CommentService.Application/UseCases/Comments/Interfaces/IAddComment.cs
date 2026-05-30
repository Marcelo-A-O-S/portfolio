using CommentService.Application.DTOs.Request;
namespace CommentService.Application.UseCases.Comments.Interfaces
{
    public interface IAddComment
    {
        Task ExecuteAsync(Guid authenticatedUserId, CommentRequest commentRequest);
    }
}