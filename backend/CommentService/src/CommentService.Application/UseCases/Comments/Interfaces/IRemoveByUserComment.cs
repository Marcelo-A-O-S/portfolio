namespace CommentService.Application.UseCases.Comments.Interfaces
{
    public interface IRemoveByUserComment
    {
        Task ExecuteAsync(Guid authenticatedUserId, Guid commentId);
    }
}