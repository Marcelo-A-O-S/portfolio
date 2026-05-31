namespace CommentService.Application.UseCases.Comments.Interfaces
{
    public interface IRemoveReply
    {
        Task ExecuteAsync(Guid authenticatedUserId, Guid commentId, Guid replyId);
    }
}