using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Domain.Queries;
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
        public async Task<PaginatedResult<LinkView>> GetByPagination(int page, Guid? toolId, Guid? postId, string? search, int itemsPage = 10)
        {
            var query = this.context.Links
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(l =>
                        l.Descriptions.Any(d =>
                            EF.Functions.Like(d.Title, $"%{search}%")
                        ));
            }
            if (toolId.HasValue)
            {
                query = query.Where(l => l.ToolId == toolId);
            }
            if (postId.HasValue)
            {
                query = query.Where(l => l.PostId == postId);
            }
            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(l => l.CreatedAt)
                .Include(l => l.LinkType)
                .Skip((page - 1) * itemsPage)
                .Take(itemsPage)
                .Select(l=> new LinkView
                {
                    Id = l.Id,
                    Url = l.Url,
                    ToolId = l.ToolId,
                    PostId = l.PostId,
                    Descriptions = l.Descriptions.Select(d=> new LinkDescriptionView
                    {
                        Id = d.Id,
                        Title = d.Title,
                        LanguageId = d.LanguageId,
                        Language = new LanguageView
                        {
                            Id = d.Language.Id,
                            Name = d.Language.Name,
                            Code = d.Language.Code
                        }
                    }).ToList(),
                    LinkTypeId = l.LinkTypeId,
                    LinkType = new LinkTypeView
                    {
                        Id = l.LinkType.Id,
                        Name = l.LinkType.Name,
                        TextColor = l.LinkType.TextColor,
                        BackgroundColor = l.LinkType.BackgroundColor,
                        BorderColor = l.LinkType.BorderColor,
                        Icon = l.LinkType.Icon
                    }
                })
                .ToListAsync();
            return new PaginatedResult<LinkView>
            {
                Items = items,
                TotalItems = totalItems,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)itemsPage)
            };
        }

        public async Task<Link> GetFullDataById(Guid Id)
        {
            return await context.Links
                .Include(l => l.Descriptions)
                    .ThenInclude(d => d.Language)
                .Include(l => l.LinkType)
                .FirstOrDefaultAsync(t => t.Id == Id);
        }
    }
}