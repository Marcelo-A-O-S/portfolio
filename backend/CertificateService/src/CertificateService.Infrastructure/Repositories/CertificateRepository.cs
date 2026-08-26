using CertificateService.Domain.Entities;
using CertificateService.Domain.Interfaces;
using CertificateService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
namespace CertificateService.Infrastructure.Repositories
{
    public class CertificateRepository : Generics<Certificate>, ICertificateRepository
    {
        private readonly DBContext context;
        public CertificateRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }

        public async Task<PaginatedResult<Certificate>> GetByPagination(int page, string? search, int itemsPage = 10)
        {
            var query = this.context.Certificates
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c =>
                        EF.Functions.Like(c.Title, $"%{search}%") ||
                        EF.Functions.Like(c.Description, $"%{search}%")||
                        EF.Functions.Like(c.Institution, $"%{search}%"));
            }
            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(c => c.CreatedAt)
                .Include(c => c.MediaProjection)
                .ToListAsync();
            return new PaginatedResult<Certificate>
            {
                Items = items,
                TotalItems = totalItems,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)itemsPage)
            };
        }

        public async Task<Certificate> GetCertificateById(Guid certificateId)
        {
            var item = await this.context.Certificates
                .AsNoTracking()
                .AsSplitQuery()
                .Where(c => c.Id == certificateId)
                .Include(c => c.MediaProjection)
                .FirstOrDefaultAsync();
            return item;
        }
    }
}