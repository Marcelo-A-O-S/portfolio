using CommentService.Domain.Entities;
using CommentService.Domain.Interfaces;
using CommentService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
namespace CommentService.Infrastructure.Repositories
{
    public class LikeRepository : Generics<Like>, ILikeRepository
    {
        private readonly DBContext context;
        public LikeRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }

        public async Task DeleteByCommentId(Guid commentId)
        {
            var likes = await this.context.Likes.Where(l => l.TargetId == commentId).ToListAsync();
            this.context.RemoveRange(likes);
            await this.context.SaveChangesAsync();
        }
    }
}