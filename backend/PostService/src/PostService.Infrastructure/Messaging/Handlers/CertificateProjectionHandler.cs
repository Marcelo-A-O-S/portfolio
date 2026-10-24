using PostService.Infrastructure.Messaging.Handlers.Interfaces;

namespace PostService.Infrastructure.Messaging.Handlers
{
    public class CertificateProjectionHandler : ICertificateProjectionHandler
    {
        public CertificateProjectionHandler()
        {
            
        }
        public Task HandleCertificateAdded(string message)
        {
            throw new NotImplementedException();
        }
    }
}