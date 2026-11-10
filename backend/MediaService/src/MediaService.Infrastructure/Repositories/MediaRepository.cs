using System.Linq.Expressions;
using MediaService.Domain.Entities;
using MediaService.Domain.Enums;
using MediaService.Domain.Interfaces;
using MediaService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
namespace MediaService.Infrastructure.Repositories
{
    public class MediaRepository : Generics<Media>, IMediaRepository
    {
        private readonly DBContext context;
        public MediaRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }
        public async Task<List<Media>> ListExpiredUncommittedMediaAsync()
        {
            var limitDate = DateTime.UtcNow.AddDays(-3);
            var mediasNoCommit = await this.context.Medias
                .Include(mf => mf.MediaOwners)
                .Where(mf => mf.Status == Status.Uploaded && mf.CreatedAt <= limitDate)
                .ToListAsync();
            return mediasNoCommit;
        }

        public async Task<Media> GetByPath(string path)
        {
            return await this.context.Medias.Where(media => media.Path == path).FirstOrDefaultAsync();
        }

        public async Task<Media> GetMediaBy(Expression<Func<Media, bool>> predicate)
        {
            return await this.context.Medias
                .Include(m => m.MediaOwners)
                .Where(predicate)
                .FirstOrDefaultAsync();
        }
    }
}