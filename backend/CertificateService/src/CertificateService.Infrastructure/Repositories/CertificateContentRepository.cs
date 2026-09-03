using CertificateService.Domain.Entities;
using CertificateService.Domain.Interfaces;
using CertificateService.Infrastructure.Context;

namespace CertificateService.Infrastructure.Repositories
{
    public class CertificateContentRepository : Generics<CertificateContent>, ICertificateContentRepository
    {
        public CertificateContentRepository(DBContext _context) : base(_context)
        {
        }
    }
}