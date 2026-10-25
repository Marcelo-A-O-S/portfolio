using PostService.Domain.Queries;

namespace PostService.Infrastructure.Messaging.Events
{
    public class CertificateUpdatedEvent
    {
        public Guid CertificateId { get; set; }
        public ICollection<CertificateContentView> CertificateContents { get; set; }
    }
}