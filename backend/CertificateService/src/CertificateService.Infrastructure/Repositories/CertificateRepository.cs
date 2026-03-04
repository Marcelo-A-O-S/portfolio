using CertificateService.Domain.Entities;
using CertificateService.Domain.Interfaces;
using CertificateService.Infrastructure.Context;
namespace CertificateService.Infrastructure.Repositories
{
    public class CertificateRepository : Generics<Certificate>, ICertificateRepository
    {
        public CertificateRepository(DBContext _context) : base(_context)
        {
        }
    }
}