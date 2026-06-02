using CertificateService.Domain.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;
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
        public DateTime UpdateAt { get; private set; }
        public DateTime IssueDate { get; private set; }
        public string? CredentialId { get; private set; }
        public string? VerificationUrl { get; private set; }
        public string Institution { get; private set;}
        public int? WorkloadHours { get; private set; }
        public Status Status { get; private set;}
        public CertificateType CertificateType { get; private set; }
        public ICollection<CertificatePost> Posts { get; private set; }
        public Certificate(string title, string description, string institution, Status status)
        {
            this.Title = title;
            this.Description = description;
            this.Institution = institution;
            this.Status = status;
            this.CreatedAt = DateTime.UtcNow;
            this.UpdateAt = DateTime.UtcNow;
        }
        public void AddImgUrl(string imgUrl, Guid mediaId)
        {
            this.ImgUrl = imgUrl;
            this.MediaFileId = mediaId;
        }
    }
}