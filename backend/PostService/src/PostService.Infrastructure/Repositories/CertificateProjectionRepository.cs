using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;

namespace PostService.Infrastructure.Repositories
{
    public class CertificateProjectionRepository : Generics<CertificateProjection>, ICertificateProjectionRepository
    {
        private readonly DBContext context;
        public CertificateProjectionRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }

        public async Task<bool> CheckAssociationWithPost(Guid certificateId, Guid postId)
        {
            return await this.context.CertificateProjections
                .AnyAsync(cp =>
                    cp.CertificateId == certificateId &&
                    cp.Posts.Any(p => p.Id == postId));
        }

        public async Task<CertificateProjection> FindWithContentBy(Expression<Func<CertificateProjection,bool>> predicate)
        {
            return await this.context.CertificateProjections
                .Include(cp => cp.CertificateContents)
                    .ThenInclude(cc => cc.Language)
                .Include(cp => cp.Posts)
                .Where(predicate)
                .FirstOrDefaultAsync();
        }

        public async Task<CertificateProjection> FindWithPostBy(Expression<Func<CertificateProjection, bool>> predicate)
        {
            return await this.context.CertificateProjections
                .Include(cp => cp.Posts)
                .Where(predicate)
                .FirstOrDefaultAsync();
        }
    }
}