using System.ComponentModel.DataAnnotations;
using CertificateService.Domain.Enums;
using CertificateService.Application.Validations;
using Microsoft.AspNetCore.Http;
namespace CertificateService.Application.DTOs.Requests
{
    public class CertificateRequest
    {
        [Required( ErrorMessage = "O titulo para o certificado é obrigatório")]
        public string Title { get; private set;}
        [Required( ErrorMessage = "A descrição do certificado é obrigatório")]
        public string Description { get; private set;}
        [Required( ErrorMessage ="A imagem principal do projeto é obrigatório.")]
        public string? ImgUrl { get; set; }
        [MaxFileSize(2 * 1024 * 1024, ErrorMessage = "O tamanho maximo aceito de imagem é 2 MB.")]
        [AllowedExtension(new[] { ".jpg", ".png", ".jpeg" })]
        [ValidationImage]
        public IFormFile? ImgFile { get; set; }
        public string? CredentialId { get; set; }
        public string? VerificationUrl { get; set; }
        [Required( ErrorMessage ="A nome da instituição do certificado é obrigatório.")]
        public string Institution { get; set;}
        public int? WorkloadHours { get; set; } 
        [Required( ErrorMessage ="A status do certificado é obrigatório.")]
        public Status Status { get; set; }
        [Required( ErrorMessage ="A tipo do certificado é obrigatório.")]
        public CertificateType  CertificateType { get; set; }
        [Required( ErrorMessage ="A data de emissão do certificado é obrigatório.")]
        public DateTime IssueDate { get; private set; }
        
    }
}