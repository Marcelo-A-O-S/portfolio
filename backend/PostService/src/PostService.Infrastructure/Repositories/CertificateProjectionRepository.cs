using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;

namespace PostService.Infrastructure.Repositories
{
    public class CertificateProjectionRepository : Generics<CertificateProjection>, ICertificateProjectionRepository
    {
        public CertificateProjectionRepository(DBContext _context) : base(_context)
        {
        }
    }
}