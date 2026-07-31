namespace CommentService.Application.UseCases.Comments.Interfaces
{
    public interface IRemoveByModeratorComment
    {
        Task ExecuteAsync(Guid authenticatedUserId, Guid commentId);
    }
}