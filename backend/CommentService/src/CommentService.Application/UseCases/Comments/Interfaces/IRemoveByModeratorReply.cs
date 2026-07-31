namespace CommentService.Application.UseCases.Comments.Interfaces
{
    public interface IRemoveByModeratorReply
    {
        Task ExecuteAsync(Guid authenticatedUserId, Guid commentId, Guid replyId);
    }
}