namespace PostService.Infrastructure.Messaging.Handlers.Interfaces
{
    public interface ILikeProjectionHandler
    {
        Task HandleLikeAdded(string message);
        Task HandleLikeRemoved(string message);
    }
}