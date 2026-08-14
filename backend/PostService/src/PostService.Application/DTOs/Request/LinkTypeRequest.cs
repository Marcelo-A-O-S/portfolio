using System.ComponentModel.DataAnnotations;

namespace PostService.Application.DTOs.Request
{
    public class LinkTypeRequest
    {
        [Required( ErrorMessage = "O nome do tipo é obrigatório.")]
        public string Name { get; set; }
        [Required( ErrorMessage = "A cor do background é obrigatório.")]
        public string BackgroundColor { get; set; }
        [Required( ErrorMessage = "A cor do texto é obrigatório.")]
        public string TextColor { get; set; }
        [Required( ErrorMessage = "O icone é obrigatório.")]
        public string Icon { get; set; }
    }
}