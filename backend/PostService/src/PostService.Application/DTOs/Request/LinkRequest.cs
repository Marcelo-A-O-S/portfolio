using System.ComponentModel.DataAnnotations;

namespace PostService.Application.DTOs.Request
{
    public class LinkRequest
    {
        public Guid? Id { get; set; }
        [Required( ErrorMessage ="O url é obrigatório.")]
        public string Url { get; set; }
        [Required( ErrorMessage ="O titulo é obrigatório.")]
        public string Title { get; set; }
        [Required( ErrorMessage ="O tipo de link é obrigatório.")]
        public LinkTypeRequest LinkType { get; set; }
    }
}