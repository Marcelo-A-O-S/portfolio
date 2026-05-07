using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using PostService.API.Validations;
using PostService.Application.Validations;
using PostService.Domain.Enums;

namespace PostService.Application.DTOs.Request
{
    public class PostRequest
    {
        [Required( ErrorMessage ="A imagem principal do projeto é obrigatório.")]
        public string? ImgUrl { get; set; }
        [MaxFileSize(2 * 1024 * 1024, ErrorMessage = "O tamanho maximo aceito de imagem é 2 MB.")]
        [AllowedExtension(new[] { ".jpg", ".png", ".jpeg" })]
        [ValidationImage]
        public IFormFile? ImgFile { get; set; }
        [Required( ErrorMessage ="O titulo do projeto é obrigatório.")]
        public string Tools { get; set; }
        [Required( ErrorMessage ="A lista das categorias é obrigatória.")]
        public string Categories { get; set; }
        [Required( ErrorMessage = "A lista de conteúdo é obrigatória.")]
        public string PostContents {get; set; }
        [Required ( ErrorMessage = "O Status do conteúdo é obrigatório.")]
        public Status Status { get; private set; }
    }
}