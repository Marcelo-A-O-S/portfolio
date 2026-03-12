using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;

namespace PostService.Infrastructure.Repositories
{
    public class CategoryRepository : Generics<Category>, ICategoryRepository
    {
        private readonly DBContext context;
        public CategoryRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }

        public async Task<PaginatedResult<Category>> GetByPagination(int page, string? language, string? search, int itemsPage = 10)
        {
            var query = this.context.Categories.AsQueryable();
            if (!string.IsNullOrWhiteSpace(language) || !string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c =>
                    c.CategoryContents.Any(cc =>
                        (string.IsNullOrWhiteSpace(language) || cc.Language == language) &&
                        (
                            string.IsNullOrWhiteSpace(search) ||
                            EF.Functions.Like(cc.Name, $"%{search}%") ||
                            EF.Functions.Like(cc.Slug, $"%{search}%")
                        )
                    )
                );
            }
            var totalItems = await query.CountAsync();
            var items = await query.Skip((page - 1) * itemsPage)
            .Take(itemsPage)
            .Include(c => c.CategoryContents
                .Where(cc => language == null || cc.Language == language))
            .ToListAsync();
            return new PaginatedResult<Category>
            {
                Items = items,
                TotalItems = totalItems,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)itemsPage)
            };
        }
    }
}