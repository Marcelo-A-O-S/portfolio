using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;
namespace PostService.Infrastructure.Repositories
{
    public class LanguageRepository : Generics<Language>, ILanguageRepository
    {
        private readonly DBContext context;
        public LanguageRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }
        public async Task<PaginatedResult<Language>> GetPagination(int page, string? search, string? code, int itemsPage = 10)
        {
            var query = this.context.Languages
                .AsNoTracking()
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(lg =>
                        (
                            string.IsNullOrWhiteSpace(search) ||
                            EF.Functions.Like(lg.Name, $"%{search}%")
                        ));
            }
            var totalItems = await query.CountAsync();
            var items =  await query
                .OrderByDescending(l => l.CreatedAt)
                .Skip((page - 1) * itemsPage)
                .Take(itemsPage)
                .ToListAsync();
            return new PaginatedResult<Language>
            {
                Items = items,
                TotalItems = totalItems,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)itemsPage)
            };
        }
    }
}