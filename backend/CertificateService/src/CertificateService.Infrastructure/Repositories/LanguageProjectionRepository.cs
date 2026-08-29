using CertificateService.Domain.Entities;
using CertificateService.Domain.Interfaces;
using CertificateService.Infrastructure.Context;

namespace CertificateService.Infrastructure.Repositories
{
    public class LanguageProjectionRepository : Generics<LanguageProjection>, ILanguageProjectionRepository
    {
        public LanguageProjectionRepository(DBContext _context) : base(_context)
        {
        }
    }
}