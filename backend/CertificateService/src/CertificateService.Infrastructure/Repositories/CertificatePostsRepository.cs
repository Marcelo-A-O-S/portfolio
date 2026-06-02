using CertificateService.Domain.Entities;
using CertificateService.Domain.Interfaces;
using CertificateService.Infrastructure.Context;
namespace CertificateService.Infrastructure.Repositories
{
    public class CertificatePostsRepository : Generics<CertificatePost>, ICertificatePostsRepository
    {
        public CertificatePostsRepository(DBContext _context) : base(_context)
        {
        }
    }
}