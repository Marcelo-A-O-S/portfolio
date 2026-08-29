using CertificateService.Domain.Entities;
using CertificateService.Domain.Interfaces;
using CertificateService.Infrastructure.Context;

namespace CertificateService.Infrastructure.Repositories
{
    public class PostProjectionRepository : Generics<PostProjection>, IPostProjectionRepository
    {
        public PostProjectionRepository(DBContext _context) : base(_context)
        {
        }
    }
}