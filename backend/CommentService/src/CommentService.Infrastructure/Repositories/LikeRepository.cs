using CommentService.Domain.Entities;
using CommentService.Domain.Interfaces;
using CommentService.Infrastructure.Context;
namespace CommentService.Infrastructure.Repositories
{
    public class LikeRepository : Generics<Like>, ILikeRepository
    {
        public LikeRepository(DBContext _context) : base(_context)
        {
        }
    }
}