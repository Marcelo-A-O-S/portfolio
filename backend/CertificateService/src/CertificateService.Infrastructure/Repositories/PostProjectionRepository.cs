using System.Linq.Expressions;
using CertificateService.Domain.Entities;
using CertificateService.Domain.Interfaces;
using CertificateService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
namespace CertificateService.Infrastructure.Repositories
{
    public class PostProjectionRepository : Generics<PostProjection>, IPostProjectionRepository
    {
        private readonly DBContext context;
        public PostProjectionRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }
        public async Task<PostProjection> FindWithContentBy(Expression<Func<PostProjection, bool>> predicate)
        {
            return await this.context.PostProjections
                .Include(pp => pp.PostContents)
                    .ThenInclude(pcp => pcp.LanguageProjection)
                .Where(predicate)
                .FirstOrDefaultAsync();
        }

        public async Task<PostProjection> GetAllBy(Expression<Func<PostProjection, bool>> predicate)
        {
            return await this.context.PostProjections
                .Include(pp => pp.Certificates)
                .Include(pp => pp.MediaProjection)
                .Include(pp => pp.PostContents)
                    .ThenInclude(pcp => pcp.LanguageProjection)
                .Where(predicate)
                .FirstOrDefaultAsync();
        }
    }
}