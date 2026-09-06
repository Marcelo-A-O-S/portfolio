using CertificateService.Domain.Entities;
using CertificateService.Domain.Interfaces;
using CertificateService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
namespace CertificateService.Infrastructure.Repositories
{
    public class LanguageProjectionRepository : Generics<LanguageProjection>, ILanguageProjectionRepository
    {
        private readonly DBContext context;
        public LanguageProjectionRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }
        public async Task DeleteByLanguageId(Guid languageId)
        {
            var entity = await this.context.LanguageProjections
                .FirstOrDefaultAsync(lp => lp.LanguageId == languageId);
            if(entity != null)
                this.context.Remove(entity);
        }
        public async Task<LanguageProjection> GetByLanguageId(Guid languageId)
        {
            return await this.context.LanguageProjections
                .FirstOrDefaultAsync(lp => lp.LanguageId == languageId);
        }
    }
}