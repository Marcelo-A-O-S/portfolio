using System.ComponentModel.DataAnnotations;
using PostService.Application.Validations;
namespace PostService.Application.DTOs.Request
{
    public class LinkTypeRequest
    {
        public Guid? Id { get; set; }
        [Required( ErrorMessage = "O nome do tipo é obrigatório.")]
        public string Name { get; set; }
        [Required( ErrorMessage = "A cor do background é obrigatório.")]
        public string BackgroundColor { get; set; }
        [Required( ErrorMessage = "A cor do texto é obrigatório.")]
        public string TextColor { get; set; }
        [Required( ErrorMessage = "A cor da borda é obrigatório.")]
        public string BorderColor { get; set; }
        [Required( ErrorMessage = "O icone é obrigatório.")]
        public string Icon { get; set; }
    }
}