namespace CertificateService.Domain.Entities
{
    public class CertificatePost
    {
        public Guid Id { get; private set; }
        public Guid CertificateId { get; private set;}
        public Certificate certificate { get; private set; }
        public Guid PostId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public CertificatePost(Guid certificateId, Guid postId)
        {
            this.CertificateId = certificateId;
            this.PostId = postId;
            this.CreatedAt = DateTime.UtcNow;
        }
    }
}