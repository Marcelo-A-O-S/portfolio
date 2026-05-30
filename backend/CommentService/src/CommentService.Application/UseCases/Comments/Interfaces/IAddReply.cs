using CommentService.Application.DTOs.Request;
using CommentService.Domain.Entities;
namespace CommentService.Application.UseCases.Comments.Interfaces
{
    public interface IAddReply
    {
        Task ExecuteAsync(Guid authenticatedUserId, Guid commentId, CommentRequest commentRequest); 
    }
}