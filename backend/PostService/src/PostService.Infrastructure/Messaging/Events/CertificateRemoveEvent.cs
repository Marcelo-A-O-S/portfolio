namespace PostService.Infrastructure.Messaging.Events
{
    public class CertificateRemoveEvent
    {
        public Guid PostId { get; set; }
        public Guid CertificateId { get; set; }
    }
}