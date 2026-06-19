using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;

namespace PostService.Infrastructure.Repositories
{
    public class MediaProjectionRepository : Generics<MediaProjection>, IMediaProjectionRepository
    {
        private readonly DBContext context;
        public MediaProjectionRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }

        public async Task<MediaProjection> GetByUrl(string url)
        {
            return await this.context.MediaProjections
                    .Where(mp => mp.Url == url).FirstOrDefaultAsync();
        }
    }
}