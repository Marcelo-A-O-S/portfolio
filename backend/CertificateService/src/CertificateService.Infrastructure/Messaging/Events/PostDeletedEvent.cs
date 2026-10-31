namespace CertificateService.Infrastructure.Messaging.Events
{
    public class PostDeletedEvent
    {
        public Guid PostId { get; set; }
    }
}