using CertificateService.Domain.Entities;
using CertificateService.Domain.Interfaces;
using CertificateService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
namespace CertificateService.Infrastructure.Repositories
{
    public class MediaFilesRepository : Generics<MediaFile>, IMediaFilesRepository
    {
        private readonly DBContext context;
        public MediaFilesRepository(DBContext _context) : base(_context)
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