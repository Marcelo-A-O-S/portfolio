using CertificateService.Domain.Entities;

namespace CertificateService.Application.Interfaces
{
    public interface ICertificateServices : IServices<Certificate>
    {
        Task<PaginatedResult<Certificate>> GetByPagination(int page, string? search, int itemsPage = 10);
        Task<Certificate> GetCertificateById(Guid certificateId);
    }
}