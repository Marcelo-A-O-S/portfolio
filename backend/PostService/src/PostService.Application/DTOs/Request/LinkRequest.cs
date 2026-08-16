using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace PostService.Application.DTOs.Request
{
    public class LinkRequest
    {
        public Guid? Id { get; set; }
        [Required( ErrorMessage ="O url é obrigatório.")]
        public string Url { get; set; }
        [Required( ErrorMessage ="O titulo é obrigatório.")]
        public string Title { get; set; }
        [Required( ErrorMessage = "O identificador do tipo de link é obrigatório.")]
        public Guid LinkTypeId { get; set; }
        public Guid? ToolId { get; set; }
        public Guid? PostId { get; set; }
    }
}