using CertificateService.Domain.Entities;

namespace CertificateService.Application.Interfaces
{
    public interface ILanguageProjectionServices : IServices<LanguageProjection>
    {
        Task<LanguageProjection> GetByLanguageId(Guid languageId);
    }
}