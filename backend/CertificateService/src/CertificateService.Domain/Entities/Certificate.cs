using CertificateService.Domain.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.ConstrainedExecution;
namespace CertificateService.Domain.Entities
{
    public class Certificate
    {
        public Guid Id { get; private set;}
        public string Title { get; private set;}
        public string Description { get; private set;}
        public string ImgUrl { get; private set; }
        public Guid? MediaFileId { get; private set; }
        public DateTime CreatedAt { get; private set;}
        public DateTime UpdatedAt { get; private set; }
        public DateTime IssuerDate { get; private set; }
        public string? CredentialId { get; private set; }
        public string? VerificationUrl { get; private set; }
        public string Institution { get; private set;}
        public int? WorkLoadHours { get; private set; }
        public Status Status { get; private set;}
        public CertificateType CertificateType { get; private set; }
        public ICollection<CertificatePost>? Posts { get; private set; }
        public Certificate(
            string title, 
            string description, 
            string institution, 
            Status status, 
            CertificateType certificateType, 
            DateTime issuerDate,
            string? credentialId,
            string? verificationUrl,
            int? workLoadHours)
        {
            this.Title = title;
            this.Description = description;
            this.Institution = institution;
            this.Status = status;
            this.CertificateType = certificateType;
            this.IssuerDate = issuerDate;
            this.CredentialId = credentialId;
            this.VerificationUrl = verificationUrl;
            this.WorkLoadHours = workLoadHours;
            this.CreatedAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;
        }
        public void AddImgUrl(string imgUrl, Guid mediaId)
        {
            this.ImgUrl = imgUrl;
            this.MediaFileId = mediaId;
        }
        public void Update(
            string title, 
            string description, 
            string institution, 
            Status status, 
            CertificateType certificateType, 
            DateTime issuerDate,
            string? credentialId,
            string? verificationUrl,
            int? workLoadHours)
        {
            this.Title = title;
            this.Description = description;
            this.Institution = institution;
            this.Status = status;
            this.CertificateType = certificateType;
            this.IssuerDate = issuerDate;
            this.UpdatedAt = DateTime.UtcNow;
            this.CredentialId = credentialId;
            this.VerificationUrl = verificationUrl;
            this.WorkLoadHours = workLoadHours;
        }
    }
}