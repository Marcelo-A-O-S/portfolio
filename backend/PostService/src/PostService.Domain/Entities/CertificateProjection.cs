using System.Runtime.ConstrainedExecution;
namespace PostService.Domain.Entities
{
    public class CertificateProjection
    {
        public Guid Id { get; set; }
        public Guid CertificateId { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public ICollection<CertificateContentProjection> CertificateContents { get; set; }
        public CertificateProjection(
            Guid _certificateId,
            Guid _postId
        )
        {
            this.CertificateId = _certificateId;
            this.PostId = _postId;
            this.CertificateContents = new List<CertificateContentProjection>();
        }
        public void AddCertificateContentProjection(CertificateContentProjection certificateContentProjection)
        {
            if(this.CertificateContents == null)
                throw new Exception("Lista não inicializada!");
            this.CertificateContents.Add(certificateContentProjection);
        }
        public void RemoveCertificateContentProjection(CertificateContentProjection certificateContentProjection)
        {
            if(this.CertificateContents == null)
                throw new Exception("Lista não inicializada!");
            this.CertificateContents.Remove(certificateContentProjection);
        }
    }
}