using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;

namespace PostService.Infrastructure.Repositories
{
    public class MediaFileRepository : Generics<MediaFile>, IMediaFileRepository
    {
        public MediaFileRepository(DBContext _context) : base(_context)
        {
        }
    }
}