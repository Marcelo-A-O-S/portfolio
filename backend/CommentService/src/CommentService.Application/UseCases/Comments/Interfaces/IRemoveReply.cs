namespace CommentService.Application.UseCases.Comments.Interfaces
{
    public interface IRemoveReply
    {
        Task ExecuteAsync(Guid authenticatedUserId, string role, Guid commentId, Guid replyId);
    }
}