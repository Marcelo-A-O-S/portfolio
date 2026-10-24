namespace PostService.Infrastructure.Messaging.Events
{
    public class CertificateAddedEvent
    {
        public Guid PostId { get; set; }
        public Guid CertificateId { get; set; }
    }
}