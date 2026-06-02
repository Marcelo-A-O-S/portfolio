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

        public async Task<MediaFile> GetByPath(string path)
        {
            return await this.context.MediaFiles.Where(media => media.Path == path).FirstOrDefaultAsync();
        }
    }
}