namespace PostService.Infrastructure.Messaging.Handlers.Interfaces
{
    public interface ICommentProjectionHandler
    {
        Task HandleCommentAdded(string message);
        Task HandleCommentRemoved(string message);
    }
}