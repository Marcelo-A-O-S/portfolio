using MediaService.Domain.Entities;
using MediaService.Domain.Interfaces;
using MediaService.Infrastructure.Context;

namespace MediaService.Infrastructure.Repositories
{
    public class MediaOwnerRepository : Generics<MediaOwner>, IMediaOwnerRepository
    {
        public MediaOwnerRepository(DBContext _context) : base(_context)
        {
        }
    }
}