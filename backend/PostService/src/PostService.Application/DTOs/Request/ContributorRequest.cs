using System.ComponentModel.DataAnnotations;
namespace PostService.Application.DTOs.Request
{
    public class ContributorRequest
    {
        public Guid? UserId { get;  set; }
        [Required(ErrorMessage ="O identificador do projeto é obrigatório.")]
        public Guid PostId { get;  set; }
        [Required(ErrorMessage ="O nome do contribuidor é obrigatório.")]
        public string Name { get;  set; }
        public string? Description { get;  set; }
        public string? ProfileUrl { get;  set;} 
    }
}