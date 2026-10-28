using System.Runtime.ConstrainedExecution;
using System.Text.Json.Serialization;
namespace PostService.Domain.Entities
{
    public class CertificateProjection
    {
        public Guid Id { get; private set; }
        public Guid CertificateId { get; private set; }
        [JsonIgnore]
        public ICollection<Post> Posts { get; private set; }
        public ICollection<CertificateContentProjection> CertificateContents { get; private set; }
        public CertificateProjection(
            Guid certificateId
        )
        {
            this.CertificateId = certificateId;
            this.Posts = new List<Post>();
            this.CertificateContents = new List<CertificateContentProjection>();
        }
        public void GenerateId()
        {
            this.Id = Guid.NewGuid();
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
        public void AddPost(Post post)
        {
            if(this.Posts == null)
                throw new Exception("Lista não inicializada");
            this.Posts.Add(post);
        }
        public void RemovePost(Post post)
        {
            if(this.Posts == null)
                throw new Exception("Lista não inicializada");
            this.Posts.Remove(post);
        }
    }
}