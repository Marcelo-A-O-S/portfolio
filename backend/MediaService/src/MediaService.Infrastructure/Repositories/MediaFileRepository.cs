using MediaService.Domain.Entities;
using MediaService.Domain.Interfaces;
using MediaService.Infrastructure.Context;
namespace MediaService.Infrastructure.Repositories
{
    public class MediaFileRepository : Generics<MediaFile>, IMediaFileRepository
    {
        public MediaFileRepository(DBContext _context) : base(_context)
        {
        }
    }
}