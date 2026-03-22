using PostService.Domain.Interfaces;
using PostService.Domain.Entities;
using PostService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace PostService.Infrastructure.Repositories
{
    public class ToolsRepository : Generics<Tool>, IToolsRepository
    {
        private readonly DBContext context;
        public ToolsRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }
        public async Task<PaginatedResult<Tool>> GetByPagination(int page, string? search, int itemsPage = 10)
        {
            var query = this.context.Tools
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(t =>
                    t.ToolContents.Any(tl =>
                        EF.Functions.Like(tl.Name, $"%{search}%") ||
                        EF.Functions.Like(tl.Slug, $"%{search}%") ||
                        EF.Functions.Like(tl.Content, $"%{search}%") ||
                        EF.Functions.Like(tl.Description, $"%{search}%")
                        )
                    );
            }
            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(t => t.CreatedAt)
                .Include(t => t.ToolContents)
                    .ThenInclude(tl => tl.Language)
                .Include(t => t.Categories)
                    .ThenInclude(c => c.CategoryContents)
                        .ThenInclude(cc => cc.Language)
                .Skip((page - 1) * itemsPage)
                .Take(itemsPage)
                .ToListAsync();
            return new PaginatedResult<Tool>
            {
                Items = items,
                TotalItems = totalItems,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)itemsPage)
            };
        }
        public async Task<Tool> GetToolById(Guid Id)
        {
            var item  = await this.context.Tools
                .AsNoTracking()
                .AsSplitQuery()
                .Where(t => t.Id == Id)
                .Include(t => t.ToolContents)
                    .ThenInclude(tl => tl.Language)
                .Include(t => t.Categories)
                    .ThenInclude(c => c.CategoryContents)
                        .ThenInclude(cc => cc.Language)
                .FirstOrDefaultAsync();
            return item;
        }
    }
}