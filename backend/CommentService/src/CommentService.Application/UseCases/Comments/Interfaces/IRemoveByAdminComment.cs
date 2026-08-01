namespace CommentService.Application.UseCases.Comments.Interfaces
{
    public interface IRemoveByAdminComment
    {
        Task ExecuteAsync(Guid authenticatedUserId, Guid commentId);
    }
}