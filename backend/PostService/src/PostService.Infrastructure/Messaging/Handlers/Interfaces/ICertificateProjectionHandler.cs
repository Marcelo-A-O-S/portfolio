namespace PostService.Infrastructure.Messaging.Handlers.Interfaces
{
    public interface ICertificateProjectionHandler
    {
        Task HandleCertificateAdded(string message);
        Task HandleCertificateRemoved(string message);
        Task HandleCertificateUpdated(string message);
    }
}