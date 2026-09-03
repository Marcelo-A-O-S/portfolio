using System.ComponentModel.DataAnnotations;

namespace CertificateService.Application.DTOs.Requests
{
    public class CertificateContentRequest
    {
        public Guid? Id {get; set; }
        public Guid LanguageId { get; set; }
        [Required(ErrorMessage = "O titulo para o certificado é obrigatório")]
        public string Title { get; set; }
        [Required(ErrorMessage = "A descrição do certificado é obrigatório")]
        public string Description { get; set; }
    }
}