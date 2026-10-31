namespace CertificateService.Domain.Queries
{
    public class PostProjectionView
    {
        public Guid? Id { get; set; }
        public Guid PostId { get; set; }
        public MediaView Media { get; set; }
        public ICollection<PostContentProjectionView> PostContents { get; set; }
    }
}