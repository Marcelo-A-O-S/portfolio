namespace CommentService.Application.UseCases.Comments.Interfaces
{
    public interface IHardRemoveComment
    {
        Task ExecuteAsync(Guid authenticatedUserId, Guid commentId);
    }
}