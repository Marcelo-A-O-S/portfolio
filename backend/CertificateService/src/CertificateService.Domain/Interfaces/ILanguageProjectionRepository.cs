using CertificateService.Domain.Entities;
namespace CertificateService.Domain.Interfaces
{
    public interface ILanguageProjectionRepository : IGenerics<LanguageProjection>
    {
        Task<LanguageProjection> GetByLanguageId(Guid languageId);
    }
}