using System.ComponentModel.DataAnnotations;
using CertificateService.Application.Validations;
using CertificateService.Domain.Enums;
using Microsoft.AspNetCore.Http;
namespace CertificateService.Application.DTOs.Requests
{
    public class CertificateRequest
    {
        public Guid? Id { get; set; }
        public List<CertificateContentRequest> CertificateContents { get; set; }
        public MediaRequest? Media { get; set; }
        public string? CredentialId { get; set; }
        public string? VerificationUrl { get; set; }
        [Required(ErrorMessage = "A nome da instituição do certificado é obrigatório.")]
        public string Institution { get; set; }
        public int? WorkloadHours { get; set; }
        [Required(ErrorMessage = "A status do certificado é obrigatório.")]
        public Status Status { get; set; }
        [Required(ErrorMessage = "A tipo do certificado é obrigatório.")]
        public CertificateType CertificateType { get; set; }
        [Required(ErrorMessage = "A data de emissão do certificado é obrigatório.")]
        public DateTime IssuerDate { get; set; }

    }
}