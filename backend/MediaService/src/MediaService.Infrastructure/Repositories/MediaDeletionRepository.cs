using MediaService.Domain.Entities;
using MediaService.Domain.Interfaces;
using MediaService.Infrastructure.Context;
namespace MediaService.Infrastructure.Repositories
{
    public class MediaDeletionRepository : Generics<MediaDeletion>, IMediaDeletionRepository
    {
        public MediaDeletionRepository(DBContext _context) : base(_context)
        {
        }
    }
}