using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using MediaService.Application.Validations;
namespace MediaService.Application.DTOs.Requests
{
    public class MediaFileRequest
    {
        public Guid OwnerId { get; set; }
        public string OwnerType { get; set; } 
        [MaxFileSize(2 * 1024 * 1024, ErrorMessage = "O tamanho maximo aceito de imagem é 2 MB.")]
        [AllowedExtension(new[] { ".jpg", ".png", ".jpeg" })]
        [ValidationImage]
        [Required(ErrorMessage = "O arquivo é obrigatório")]
        public IFormFile File { get; set; }
    }
}