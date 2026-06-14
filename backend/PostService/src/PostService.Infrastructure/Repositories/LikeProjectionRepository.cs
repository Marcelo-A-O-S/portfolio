using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;

namespace PostService.Infrastructure.Repositories
{
    public class LikeProjectionRepository : Generics<LikeProjection>, ILikeProjectionRepository
    {
        public LikeProjectionRepository(DBContext _context) : base(_context)
        {
        }
    }
}