using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Domain.Queries;
using PostService.Infrastructure.Context;
namespace PostService.Infrastructure.Repositories
{
    public class ContributorRepository : Generics<Contributor>, IContributorRepository
    {
        private readonly DBContext context;
        public ContributorRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }

        public async Task<PaginatedResult<ContributorView>> GetByPagination(int page, Guid postId, string? search, int itemsPage = 10)
        {
            var query = this.context.Contributors
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable();
            query = query.Where(l => l.PostId == postId);
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c =>
                        EF.Functions.Like(c.Name, $"%{search}%"));
            }
            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * itemsPage)
                .Take(itemsPage)
                .Select(c => new ContributorView
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    PostId = c.PostId,
                    Name = c.Name,
                    Description = c.Description,
                    ProfileUrl = c.ProfileUrl
                }).ToListAsync();
            return new PaginatedResult<ContributorView>
            {
                Items = items,
                TotalItems = totalItems,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)itemsPage)
            };
        }
    }
}