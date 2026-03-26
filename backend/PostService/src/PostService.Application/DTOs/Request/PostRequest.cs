using System.ComponentModel.DataAnnotations;
using PostService.Domain.Enums;

namespace PostService.Application.DTOs.Request
{
    public class PostRequest
    {
        public Guid? Id { get; set; }
        [Required( ErrorMessage ="A imagem principal do projeto é obrigatório.")]
        public string ImgUrl { get; set; }
        [Required( ErrorMessage ="O titulo do projeto é obrigatório.")]
        public string Title { get; set; }
        [Required( ErrorMessage ="A descrição do projeto é obrigatório.")]
        public string Description { get; set; }
        [Required( ErrorMessage ="A lista de ferramentas é obrigatória.")]
        public List<ToolRequest> Tools { get; set; }
        [Required( ErrorMessage ="A lista das categorias é obrigatória.")]
        public List<CategoryRequest> Categories { get; set; }
        [Required( ErrorMessage = "A lista de conteúdo é obrigatória.")]
        public List<PostContentRequest> PostContents {get; set; }
        public Status Status { get; private set; }
    }
}