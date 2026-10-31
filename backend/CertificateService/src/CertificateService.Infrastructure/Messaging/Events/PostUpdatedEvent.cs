using CertificateService.Domain.Queries;
namespace CertificateService.Infrastructure.Messaging.Events
{
    public class PostUpdatedEvent
    {
        public Guid PostId { get; set; }
        public MediaView Media { get; set; }
        public ICollection<PostContentProjectionView> PostContents { get; set; }
    }
}