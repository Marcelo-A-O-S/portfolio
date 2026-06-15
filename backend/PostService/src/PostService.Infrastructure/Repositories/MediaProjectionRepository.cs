using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;

namespace PostService.Infrastructure.Repositories
{
    public class MediaProjectionRepository : Generics<MediaProjection>, IMediaProjectionRepository
    {
        public MediaProjectionRepository(DBContext _context) : base(_context)
        {
        }
    }
}