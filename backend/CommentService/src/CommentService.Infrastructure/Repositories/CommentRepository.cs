using CommentService.Domain.Entities;
using CommentService.Domain.Interfaces;
using CommentService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CommentService.Infrastructure.Repositories
{
    public class CommentRepository : Generics<Comment>, ICommentRepository
    {
        private readonly DBContext context;
        public CommentRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }

        public async Task<List<Comment>> GetCommentsByTargeIdsPage(List<Guid> targetIds)
        {
            return await this.context.Comments
                .Where(c => targetIds.Contains(c.TargetId))
                .ToListAsync();
        }
        public async Task<List<Comment>> GetCommentsByTargetId(Guid targetId)
        {
            return await this.context.Comments
                .AsSplitQuery()
                .OrderByDescending(p=> p.CreatedAt)
                .Where(c => c.TargetId == targetId)
                .Include(c => c.ParentComment)
                .ToListAsync();
        }

        public async Task<Dictionary<Guid, int>> GetQuantityCommentsByTargeIdsPage(List<Guid> targetIds)
        {
            return await this.context.Comments
                .Where(c => targetIds.Contains(c.TargetId))
                .GroupBy(c => c.TargetId)
                .Select(g => new
                {
                    TargetId = g.Key,
                    Count = g.Count()
                })
                .ToDictionaryAsync(
                    x => x.TargetId,
                    x => x.Count
                );     
        }
    }
}