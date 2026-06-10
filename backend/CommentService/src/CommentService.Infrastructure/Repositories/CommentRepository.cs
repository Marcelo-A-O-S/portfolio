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

        public async Task<List<Comment>> GetCommentsByPostIdsPage(List<Guid> postIds)
        {
            return await this.context.Comments
                .Where(c => postIds.Contains(c.PostId))
                .ToListAsync();
        }
        public async Task<List<Comment>> GetCommentsByPostId(Guid postId)
        {
            return await this.context.Comments
                .AsSplitQuery()
                .OrderByDescending(p=> p.CreatedAt)
                .Where(c => c.PostId == postId)
                .Include(c => c.ParentComment)
                .ToListAsync();
        }

        public async Task<Dictionary<Guid, int>> GetQuantityCommentsByPostIdsPage(List<Guid> postIds)
        {
            return await this.context.Comments
                .Where(c => postIds.Contains(c.PostId))
                .GroupBy(c => c.PostId)
                .Select(g => new
                {
                    PostId = g.Key,
                    Count = g.Count()
                })
                .ToDictionaryAsync(
                    x => x.PostId,
                    x => x.Count
                );     
        }
    }
}