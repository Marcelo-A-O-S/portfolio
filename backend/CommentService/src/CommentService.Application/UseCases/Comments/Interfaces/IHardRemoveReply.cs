namespace CommentService.Application.UseCases.Comments.Interfaces
{
    public interface IHardRemoveReply
    {
        Task ExecuteAsync(Guid authenticatedUserId, Guid commentId, Guid replyId);
    }
}