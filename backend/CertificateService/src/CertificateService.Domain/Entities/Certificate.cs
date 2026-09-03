using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;
using CertificateService.Domain.Enums;
namespace CertificateService.Domain.Entities
{
    public class Certificate
    {
        public Guid Id { get; private set; }
        public Guid? MediaProjectionId { get; private set; }
        public MediaProjection? MediaProjection { get; private set; }
        public ICollection<PostProjection> PostProjections  { get; private set;}
        public ICollection<CertificateContent> CertificateContents { get; private set; }
        public string? CredentialId { get; private set; }
        public string? VerificationUrl { get; private set; }
        public string Institution { get; private set; }
        public int? WorkLoadHours { get; private set; }
        public Status Status { get; private set; }
        public CertificateType CertificateType { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime IssuerDate { get; private set; }
        public Certificate(
            string institution,
            string? credentialId,
            string? verificationUrl,
            int? workLoadHours,
            Status status,
            CertificateType certificateType,
            DateTime issuerDate)
        {
            this.Institution = institution;
            this.Status = status;
            this.CertificateType = certificateType;
            this.IssuerDate = issuerDate;
            this.CredentialId = credentialId;
            this.VerificationUrl = verificationUrl;
            this.WorkLoadHours = workLoadHours;
            this.CreatedAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;
            this.PostProjections = new List<PostProjection>();
            this.CertificateContents = new List<CertificateContent>();
        }
        public void Update(
            string institution,
            Status status,
            CertificateType certificateType,
            DateTime issuerDate,
            string? credentialId,
            string? verificationUrl,
            int? workLoadHours)
        {
            this.Institution = institution;
            this.Status = status;
            this.CertificateType = certificateType;
            this.IssuerDate = issuerDate;
            this.UpdatedAt = DateTime.UtcNow;
            this.CredentialId = credentialId;
            this.VerificationUrl = verificationUrl;
            this.WorkLoadHours = workLoadHours;
        }
        public void AddMedia(Guid mediaProjectId)
        {
            this.MediaProjectionId = mediaProjectId;
            this.UpdatedAt = DateTime.UtcNow;
        }
        public void AddPostProjection(PostProjection postProjection)
        {
            if(this.PostProjections == null)
                throw new ValidationException("Lista de conteudo não inicializada.");
            this.PostProjections.Add(postProjection);
        }
        public void AddCertificateContent(CertificateContent certificateContent)
        {
            if(this.CertificateContents == null)
                throw new ValidationException("Lista de conteudo não inicializada.");
            this.CertificateContents.Add(certificateContent);
        }
        public void RemoveCertificateContent(CertificateContent certificateContent)
        {
            if(this.CertificateContents == null)
                throw new ValidationException("Lista de conteudo não inicializada.");
            this.CertificateContents.Remove(certificateContent);
        }
        public void ValidateCertificateContents(IEnumerable<Guid> certificateContentIds)
        {
            if(this.CertificateContents == null)
                throw new ValidationException("Lista de conteudo não inicializada.");
            var ids = certificateContentIds.ToHashSet();
            var toRemove = this.CertificateContents
                .Where(tc => !ids.Contains(tc.Id))
                .ToList();
            foreach(var certificateContent in toRemove)
                this.CertificateContents.Remove(certificateContent);
        }
    }
}