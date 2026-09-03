using CertificateService.Application.DTOs.Responses;
namespace CertificateService.Application.Interfaces
{
    public interface ILanguageServicesClient
    {
        Task<LanguageResponse> GetLanguageAsync(Guid languageId);
    }
}