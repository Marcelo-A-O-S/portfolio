using System.ComponentModel.DataAnnotations;

namespace PostService.Application.DTOs.Request
{
    public class LikeRequest
    {
        [Required]
        public Guid PostId { get; set;}
    }
}