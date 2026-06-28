namespace CommentService.Infrastructure.Messaging.Handlers.Interfaces
{
    public interface IUserEventHandler
    {
        Task RemoveCache(string message);
    }
}