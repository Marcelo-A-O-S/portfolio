using CertificateService.Domain.Enums;

namespace CertificateService.Domain.Queries
{
    public class CertificateView
    {
        public Guid? Id { get; set; }
        public MediaView Media { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? CredentialId { get; set; }
        public string? VerificationUrl { get; set; }
        public string Institution { get; set; }
        public int? WorkLoadHours { get; set; }
        public Status Status { get; set; }
        public CertificateType CertificateType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime IssuerDate { get; set; }
    }
}