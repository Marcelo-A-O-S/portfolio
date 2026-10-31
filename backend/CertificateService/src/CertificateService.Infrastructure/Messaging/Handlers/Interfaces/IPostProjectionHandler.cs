namespace CertificateService.Infrastructure.Messaging.Handlers.Interfaces
{
    public interface IPostProjectionHandler
    {
        Task HandlePostProjectionUpdated(string message);
        Task HandlePostProjectionRemoved(string message);
        Task HandlePostProjectionDeleted(string message);
    }
}