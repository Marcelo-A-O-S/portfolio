namespace CommentService.Application.UseCases.Comments.Interfaces
{
    public interface IRemoveComment
    {
        Task ExecuteAsync(Guid commentId);
    }
}