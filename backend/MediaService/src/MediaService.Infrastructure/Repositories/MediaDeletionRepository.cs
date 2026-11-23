using MediaService.Domain.Entities;
using MediaService.Domain.Interfaces;
using MediaService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
namespace MediaService.Infrastructure.Repositories
{
    public class MediaDeletionRepository : Generics<MediaDeletion>, IMediaDeletionRepository
    {
        private readonly DBContext context;
        public MediaDeletionRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }

        public async Task<List<MediaDeletion>> GetMediasDeletionProcessed()
        {
            return await this.context.MediaDeletions
                .Where(md => md.ProcessedAt != null)
                .OrderBy(md => md.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<MediaDeletion>> GetMediasPending()
        {
            return await this.context.MediaDeletions
                .Where(md => md.ProcessedAt == null)
                .OrderBy(md => md.CreatedAt)
                .ToListAsync();
        }
        
    }
}