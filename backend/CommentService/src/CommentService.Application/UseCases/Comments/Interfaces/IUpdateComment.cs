using CommentService.Domain.Entities;
using CommentService.Application.DTOs.Request;
namespace CommentService.Application.UseCases.Comments.Interfaces
{
    public interface IUpdateComment
    {
        Task ExecuteAsync(Guid commentId, CommentRequest commentRequest);
    }
}