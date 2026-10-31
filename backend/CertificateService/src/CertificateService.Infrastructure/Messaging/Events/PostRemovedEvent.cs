namespace CertificateService.Infrastructure.Messaging.Events
{
    public class PostRemovedEvent
    {
        public Guid PostId { get; set; }
    }
}