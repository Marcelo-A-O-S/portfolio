using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.DTOs.Request
{
    public class RefreshRequest
    {
        [Required(ErrorMessage = "O identificador do usuário é obrigatório.")]
        public Guid UserId { get; set;}
        [Required(ErrorMessage = "O identificador do token é obrigatório.")]
        public Guid RefreshTokenId { get; set; }
        [Required( ErrorMessage = "O refreshToken é obrigatório para revalidação")]
        public string RefreshToken {get; set;}
        [Required( ErrorMessage = "O identificador gerado para o dispositivo é obrigatório.")]
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
    }
}