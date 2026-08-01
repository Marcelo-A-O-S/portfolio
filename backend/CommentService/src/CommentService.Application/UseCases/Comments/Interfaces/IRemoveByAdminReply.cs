namespace CommentService.Application.UseCases.Comments.Interfaces
{
    public interface IRemoveByAdminReply
    {
        Task ExecuteAsync(Guid authenticatedUserId, Guid commentId, Guid replyId);
    }
}