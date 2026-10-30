using CertificateService.Infrastructure.Messaging.Handlers.Interfaces;

namespace CertificateService.Infrastructure.Messaging.Handlers
{
    public class PostProjectionHandler : IPostProjectionHandler
    {
        public Task HandlePostProjectionDeleted(string message)
        {
            throw new NotImplementedException();
        }

        public Task HandlePostProjectionUpdated(string message)
        {
            throw new NotImplementedException();
        }
    }
}