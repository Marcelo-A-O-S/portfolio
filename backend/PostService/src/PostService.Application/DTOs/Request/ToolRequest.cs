using System.ComponentModel.DataAnnotations;
using PostService.Application.Validations;
using PostService.Domain.Enums;
using PostService.API.Validations;
using Microsoft.AspNetCore.Http;
namespace PostService.Application.DTOs.Request
{
    public class ToolRequest
    {
        public string? ImgUrl { get; set; }

        [MaxFileSize(2 * 1024 * 1024, ErrorMessage = "O tamanho maximo aceito de imagem é 2 MB.")]
        [AllowedExtension(new[] { ".jpg", ".png", ".jpeg" })]
        public IFormFile? ImgFile { get; set; }
        [Required]
        public Status Status { get; set; }
        [Required]
        public string Categories { get; set; }
        [Required]
        public string ToolContents { get; set; }
    }
}