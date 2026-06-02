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

        public async Task<List<Comment>> GetCommentsByPostId(Guid postId)
        {
            return await this.context.Comments
                .AsSplitQuery()
                .OrderByDescending(p=> p.CreatedAt)
                .Where(c => c.PostId == postId)
                .Include(c => c.ParentComment)
                .ToListAsync();
        }
    }
}