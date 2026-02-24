using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.DTOs.Request
{
    public class LogoutRequest
    {
        [Required(ErrorMessage = "O identificador do usuário é obrigatório.")]
        public Guid UserId { get; set;}
        [Required( ErrorMessage = "O refreshToken é obrigatório para revalidação")]
        public string RefreshToken {get; set;}
        [Required( ErrorMessage = "O identificador gerado para o dispositivo é obrigatório.")]
        public string DeviceId { get; set;}
    }
}