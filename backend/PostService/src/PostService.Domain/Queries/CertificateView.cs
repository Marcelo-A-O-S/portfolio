namespace PostService.Domain.Queries
{
    public class CertificateView
    {
        public Guid CertificateId { get; set; }
        public Guid PostId { get; set; }
        public ICollection<CertificateContentView> CertificateContents { get; set; }
    }
}