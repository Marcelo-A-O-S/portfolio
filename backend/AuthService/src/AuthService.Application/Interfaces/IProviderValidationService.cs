using AuthService.Application.DTOs.Request;
namespace AuthService.Application.Interfaces
{
    public interface IProviderValidationService
    {
        Task<ProviderUserData> Validate(string token);
    }
}