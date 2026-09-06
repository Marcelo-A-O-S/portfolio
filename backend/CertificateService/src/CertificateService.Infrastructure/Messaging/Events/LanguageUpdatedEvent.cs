namespace CertificateService.Infrastructure.Messaging.Events
{
    public class LanguageUpdatedEvent
    {
        public Guid LanguageId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}