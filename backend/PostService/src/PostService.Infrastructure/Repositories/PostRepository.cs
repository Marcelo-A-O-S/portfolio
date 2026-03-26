using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;

namespace PostService.Infrastructure.Repositories
{
    public class PostRepository : Generics<Post>, IPostRepository
    {
        private readonly DBContext context;
        public PostRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }

        public async Task<PaginatedResult<Post>> GetByPagination(int page, string? search, int itemsPage = 10)
        {
            var query = this.context.Posts
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.PostContents.Any(pt =>
                        EF.Functions.Like(pt.Title, $"%{search}%") ||
                        EF.Functions.Like(pt.Content, $"%{search}%") ||
                        EF.Functions.Like(pt.Description, $"%{search}%")
                        )
                    );
            }
            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(p=> p.CreatedAt)
                .Include(p => p.PostContents)
                    .ThenInclude(pt => pt.Language)
                .Include(p => p.Categories)
                .Include(p => p.Tools)
                .Skip((page - 1) * itemsPage)
                .Take(itemsPage)
                .ToListAsync();
            return new PaginatedResult<Post>
            {
                Items = items,
                TotalItems = totalItems,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)itemsPage)
            };
        }
    }
}