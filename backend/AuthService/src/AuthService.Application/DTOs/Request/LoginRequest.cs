using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.DTOs.Request
{
    public class LoginRequest
    {
        [Required(ErrorMessage ="O campo email é obrigatório")]
        [EmailAddress]
        public string Email {get; set;}
        public string Name { get; set;}
        public string Username { get; set;}
        public string ProfileUrl { get; set;}
        [Required( ErrorMessage = "O provedor de autenticação é obrigatório.")]
        public string Provider { get; set;}
        [Required( ErrorMessage = "O identificador do provedor de autenticação é obrigatório.")]
        public string ProviderId {get; set;}
        [Required( ErrorMessage = "O identificador gerado para o dispositivo é obrigatório.")]
        public string DeviceId  { get; set;}
        [Required( ErrorMessage = "O nome do despositivo é obrigatório.")]
        public string DeviceName { get; set;}
    }
}