namespace CommentService.Application.UseCases.Comments.Interfaces
{
    public interface IRemoveByUserReply
    {
        Task ExecuteAsync(Guid authenticatedUserId, Guid commentId, Guid replyId);
    }
}