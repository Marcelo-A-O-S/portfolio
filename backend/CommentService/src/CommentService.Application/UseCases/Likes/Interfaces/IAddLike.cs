namespace CommentService.Application.UseCases.Likes.Interfaces
{
    public interface IAddLike
    {
        Task ExecuteAsync(Guid authenticatedUserId, Guid commentId);
    }
}