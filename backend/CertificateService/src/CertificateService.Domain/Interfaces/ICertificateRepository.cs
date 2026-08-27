using CertificateService.Domain.Entities;
using CertificateService.Domain.Queries;

namespace CertificateService.Domain.Interfaces
{
    public interface ICertificateRepository : IGenerics<Certificate>
    {
        Task<PaginatedResult<CertificateView>> GetByPagination(int page, string? search, int itemsPage = 10);
        Task<Certificate> GetCertificateById(Guid certificateId);
    }
}