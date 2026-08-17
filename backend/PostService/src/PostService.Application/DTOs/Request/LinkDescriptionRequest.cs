using System.ComponentModel.DataAnnotations;

namespace PostService.Application.DTOs.Request
{
    public class LinkDescriptionRequest
    {
        public Guid? Id { get; set; }
        [Required( ErrorMessage ="O titulo é obrigatório.")]
        public string Title { get; set; }
        [Required( ErrorMessage ="O Identificador da linguagem é obrigatório.")]
        public Guid LanguageId { get; set; }
    }
}