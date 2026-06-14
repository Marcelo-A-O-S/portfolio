namespace MediaService.Infrastructure.Messaging.Handlers.Interfaces
{
    public interface IMediaFileProjectionHandler
    {
        Task HandleMediaCommit(string message);
        Task HandleMediaDelete(string message);
    }
}