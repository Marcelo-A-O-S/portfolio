using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;
namespace PostService.Infrastructure.Repositories
{
    public class LinkRepository : Generics<Link>, ILinkRepository
    {
        private readonly DBContext context;
        public LinkRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }
        public async Task<PaginatedResult<Link>> GetByPagination(int page, string? search, int itemsPage = 10)
        {
            var query = this.context.Links
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(l =>
                        (
                            string.IsNullOrWhiteSpace(search) ||
                            EF.Functions.Like(l.Title, $"%{search}%")
                        ));
            }
            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(l => l.CreatedAt)
                .Include(l => l.LinkType)
                .Skip((page - 1) * itemsPage)
                .Take(itemsPage)
                .ToListAsync();
            return new PaginatedResult<Link>
            {
                Items = items,
                TotalItems = totalItems,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)itemsPage)
            };
        }
    }
}