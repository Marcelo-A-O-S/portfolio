namespace CertificateService.Domain.Entities
{
    public class PostProjection
    {
        public Guid Id { get; private set; }
        public ICollection<PostContentProjection> PostContents { get; private set; }
    }
}