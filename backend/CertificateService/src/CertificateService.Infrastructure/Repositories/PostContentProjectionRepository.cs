using CertificateService.Domain.Entities;
using CertificateService.Domain.Interfaces;
using CertificateService.Infrastructure.Context;

namespace CertificateService.Infrastructure.Repositories
{
    public class PostContentProjectionRepository : Generics<PostContentProjection>, IPostContentProjectionRepository
    {
        public PostContentProjectionRepository(DBContext _context) : base(_context)
        {
        }
    }
}