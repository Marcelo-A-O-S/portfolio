namespace CertificateService.Infrastructure.Messaging.Handlers.Interfaces
{
    public interface ILanguageProjectionHandler
    {
        Task HandleLanguageDeleted(string message);
        Task HandleLanguageUpdated(string message);
    }
}