using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;
using PostService.Domain.Entities;
namespace PostService.Application.DTOs.Request
{
    public class LinkRequest
    {
        public Guid? Id { get; set; }
        [Required( ErrorMessage ="O url é obrigatório.")]
        public string Url { get; set; }
        [Required( ErrorMessage ="As descrições são obrigatórias.")]
        public List<LinkDescriptionRequest> Descriptions { get; set; }
        [Required( ErrorMessage = "O identificador do tipo de link é obrigatório.")]
        public Guid LinkTypeId { get; set; }
        public Guid? ToolId { get; set; }
        public Guid? PostId { get; set; }
    }
}