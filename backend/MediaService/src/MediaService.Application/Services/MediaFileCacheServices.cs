using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace MediaService.Application.Services
{
    public class MediaFileCacheServices 
    {
        [Required]
        public IFormFile File { get; set; }
    }
}