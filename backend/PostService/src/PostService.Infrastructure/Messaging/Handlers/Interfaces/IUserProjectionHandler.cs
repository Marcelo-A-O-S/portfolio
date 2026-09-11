namespace PostService.Infrastructure.Messaging.Handlers.Interfaces
{
    public interface IUserProjectionHandler
    {
        Task RemoveUserCache(string message);
        
    }
}