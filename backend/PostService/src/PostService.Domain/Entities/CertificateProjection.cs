namespace PostService.Domain.Entities
{
    public class CertificateProjection
    {
        public Guid Id { get; set; }
        public Guid CertificateId { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public ICollection<CertificateContentProjection> CertificateContents { get; set; }
    }
}