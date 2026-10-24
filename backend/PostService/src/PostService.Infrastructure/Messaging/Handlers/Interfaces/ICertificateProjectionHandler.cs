namespace PostService.Infrastructure.Messaging.Handlers.Interfaces
{
    public interface ICertificateProjectionHandler
    {
        Task HandleCertificateAdded(string message);
    }
}