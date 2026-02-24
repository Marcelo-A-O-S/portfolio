using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;
namespace PostService.Infrastructure.Repositories
{
    public class LikeRepository : Generics<Like>, ILikeRepository
    {
        public LikeRepository(DBContext _context) : base(_context)
        {
        }
    }
}