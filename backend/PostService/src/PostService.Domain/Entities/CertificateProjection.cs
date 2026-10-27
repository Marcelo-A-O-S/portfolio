using System.Runtime.ConstrainedExecution;
namespace PostService.Domain.Entities
{
    public class CertificateProjection
    {
        public Guid Id { get; private set; }
        public Guid CertificateId { get; private set; }
        public Guid PostId { get; private set; }
        public Post Post { get; private set; }
        public ICollection<CertificateContentProjection> CertificateContents { get; private set; }
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