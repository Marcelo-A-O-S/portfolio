namespace CommentService.Application.UseCases.Likes.Interfaces
{
    public interface IRemoveLike
    {
        Task ExecuteAsync(Guid authenticatedUserId, Guid likeId, Guid commentId);
    }
}