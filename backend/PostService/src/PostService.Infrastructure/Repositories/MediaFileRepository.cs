using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;
namespace PostService.Infrastructure.Repositories
{
    public class MediaFileRepository : Generics<MediaFile>, IMediaFileRepository
    {
        private readonly DBContext context;
        public MediaFileRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }

        public async Task DeleteExpiredPendingMediaAsync()
        {
            var now = DateTime.UtcNow;
            var limitDate = DateTime.UtcNow.AddDays(-3);
            var mediasNoCommit = await this.context.MediaFiles
                .Where(mf => !mf.IsCommitted && mf.CreatedAt <= limitDate)
                .ToListAsync();
            this.context.RemoveRange(mediasNoCommit);
            await this.context.SaveChangesAsync();
        }

        public async Task<MediaFile> GetByPath(string path)
        {
            return await this.context.MediaFiles.Where(media => media.Path == path).FirstOrDefaultAsync();
        }
    }
}