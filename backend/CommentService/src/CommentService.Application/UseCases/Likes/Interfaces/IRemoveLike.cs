using CommentService.Application.DTOs.Request;
namespace CommentService.Application.UseCases.Likes.Interfaces
{
    public interface IRemoveLike
    {
        Task ExecuteAsync(Guid authenticatedUserId, LikeRequest likeRequest);
    }
}