using CertificateService.Domain.Entities;

namespace CertificateService.Domain.Interfaces
{
    public interface ICertificateRepository : IGenerics<Certificate>
    {
        Task<PaginatedResult<Certificate>> GetByPagination(int page, string? search, int itemsPage = 10);
    }
}