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
                    .ThenInclude(c => c.CategoryContents)
                        .ThenInclude(cc => cc.Language)
                .Include(p => p.Tools)
                    .ThenInclude(t => t.ToolContents)
                        .ThenInclude(tl => tl.Language)
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

        public async Task<Post> GetForUpdate(Guid Id)
        {
            return await context.Posts
                .Include(p => p.PostContents)
                .Include(p => p.Categories)
                .Include(p => p.Tools)
                .FirstOrDefaultAsync(t => t.Id == Id);
        }

        public async Task<Post> GetPostById(Guid Id)
        {
            var item  = await this.context.Posts
                .AsNoTracking()
                .AsSplitQuery()
                .Where(t => t.Id == Id)
                .Include(p => p.PostContents)
                    .ThenInclude(pt => pt.Language)
                .Include(p => p.Categories)
                    .ThenInclude(c => c.CategoryContents)
                        .ThenInclude(cc => cc.Language)
                .Include(p => p.Tools)
                    .ThenInclude(t => t.ToolContents)
                        .ThenInclude(tl => tl.Language)
                .FirstOrDefaultAsync();
            return item;
        }

        public async Task<List<Post>> GetPosts()
        {
            return await this.context.Posts
                .AsNoTracking()
                .AsSplitQuery()
                .OrderByDescending(p=> p.CreatedAt)
                .Include(p => p.PostContents)
                    .ThenInclude(pt => pt.Language)
                .Include(p => p.Categories)
                    .ThenInclude(c => c.CategoryContents)
                        .ThenInclude(cc => cc.Language)
                .Include(p => p.Tools)
                    .ThenInclude(t => t.ToolContents)
                        .ThenInclude(tl => tl.Language)
                .ToListAsync();
        }
    }
}