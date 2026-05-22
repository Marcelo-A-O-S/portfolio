using PostService.Application.DTOs.Request;
namespace PostService.Application.UseCases.Likes.Interfaces
{
    public interface IAddLike
    {
        Task ExecuteAsync(Guid userId, LikeRequest likeRequest);
    }
}