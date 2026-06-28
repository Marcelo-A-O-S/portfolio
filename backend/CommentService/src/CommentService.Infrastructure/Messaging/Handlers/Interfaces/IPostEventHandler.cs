namespace CommentService.Infrastructure.Messaging.Handlers.Interfaces
{
    public interface IPostEventHandler
    {
        Task RemoveCache(string message);
    }
}