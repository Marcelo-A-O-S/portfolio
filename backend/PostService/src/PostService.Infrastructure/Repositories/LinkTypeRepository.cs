using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;
namespace PostService.Infrastructure.Repositories
{
    public class LinkTypeRepository : Generics<LinkType>, ILinkTypeRepository
    {
        private readonly DBContext context;
        public LinkTypeRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }

        public async Task<PaginatedResult<LinkType>> GetByPagination(int page, string? search, int itemsPage = 10)
        {
            var query = this.context.LinkTypes
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(lt =>
                        (
                            string.IsNullOrWhiteSpace(search) ||
                            EF.Functions.Like(lt.Name, $"%{search}%")
                        ));
            }
            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * itemsPage)
                .Take(itemsPage)
                .ToListAsync();
            return new PaginatedResult<LinkType>
            {
                Items = items,
                TotalItems = totalItems,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)itemsPage)
            };
        }
    }
}