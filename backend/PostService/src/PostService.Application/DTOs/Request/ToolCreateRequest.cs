using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using PostService.Domain.Enums;
using PostService.API.Validations;
using PostService.Application.Validations;

namespace PostService.Application.DTOs.Request
{
    public class ToolCreateRequest
    {
        [Required]
        [MaxFileSize(2 * 1024 * 1024, ErrorMessage = "O tamanho maximo aceito de imagem é 2 MB.")]
        [AllowedExtension(new[] { ".jpg", ".png", ".jpeg" })]
        public IFormFile ImgUrl { get; set; }
        [Required]
        public Status Status { get; set; }
        [Required]
        public string Categories { get; set; }
        [Required]
        public string ToolContents { get; set; }
    }
}