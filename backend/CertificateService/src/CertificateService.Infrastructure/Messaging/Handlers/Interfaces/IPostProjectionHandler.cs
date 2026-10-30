namespace CertificateService.Infrastructure.Messaging.Handlers.Interfaces
{
    public interface IPostProjectionHandler
    {
        Task HandlePostProjectionUpdated(string message);
        Task HandlePostProjectionDeleted(string message);
    }
}