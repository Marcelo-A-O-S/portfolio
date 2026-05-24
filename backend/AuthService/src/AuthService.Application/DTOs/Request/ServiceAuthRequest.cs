using System.ComponentModel.DataAnnotations;
namespace AuthService.Application.DTOs.Request
{
    public class ServiceAuthRequest
    {
        [Required]
        public string ClientId { get; set; }
        [Required]
        public string ClientSecret { get; set; }
    }
}