using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Domain.Queries;
using PostService.Infrastructure.Context;

namespace PostService.Infrastructure.Repositories
{
    public class ToolsRepository : Generics<Tool>, IToolsRepository
    {
        private readonly DBContext context;
        public ToolsRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }

        public async Task<PaginatedResult<ToolView>> GetByPagination(Guid? authenticatedUserId, int page, string? search, int itemsPage = 10)
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
                .Select(t => new ToolView
                {
                    Id = t.Id,
                    Media = new MediaView
                    {
                        Id = t.MediaProjectionId,
                        MediaId = t.MediaProjection.MediaId,
                        Url = t.MediaProjection.Url
                    },
                    Status = t.Status,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt,
                    Likes = t.LikeCount,
                    Comments = t.CommentCount,
                    Liked = this.context.LikeProjections.Any(lp =>
                                lp.TargetId == t.Id &&
                                lp.UserId == authenticatedUserId),
                    Categories = t.Categories.Select(c => new CategoryView
                    {
                        Id = c.Id,
                        CategoryContents = c.CategoryContents.Select(cc => new CategoryContentView
                        {
                            Id = cc.Id,
                            Name = cc.Name,
                            Slug = cc.Slug,
                            CreatedAt = cc.CreatedAt,
                            UpdatedAt = cc.UpdateAt,
                            Language = new LanguageView
                            {
                                Id = cc.Language.Id,
                                Code = cc.Language.Code,
                                Name = cc.Language.Name
                            }
                        }).ToList()
                    }).ToList(),
                    ToolContents = t.ToolContents.Select(tc => new ToolContentView
                    {
                        Id = tc.Id,
                        Name = tc.Name,
                        Slug = tc.Slug,
                        Title = tc.Title,
                        CreatedAt = tc.CreatedAt,
                        Content = tc.Content,
                        Description = tc.Description,
                        Images = tc.Images.Select(img => new MediaView
                        {
                            Id = img.Id,
                            MediaId = img.MediaId,
                            Url = img.Url
                        }).ToList(),
                        Language = new LanguageView
                        {
                            Id = tc.Language.Id,
                            Code = tc.Language.Code,
                            Name = tc.Language.Name
                        }
                    }).ToList(),
                })
                .ToListAsync();
            return new PaginatedResult<ToolView>
            {
                Items = items,
                TotalItems = totalItems,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)itemsPage)
            };
        }
        public async Task<Tool> GetForUpdate(Guid Id)
        {
            return await context.Tools
                .Include(t => t.Categories)
                .Include(t => t.ToolContents)
                .FirstOrDefaultAsync(t => t.Id == Id);
        }

        public async Task<Tool> GetFullDataById(Guid Id)
        {
            return await context.Tools
                .Include(t => t.Categories)
                .Include(t => t.ToolContents)
                .FirstOrDefaultAsync(t => t.Id == Id);
        }

        public async Task<Tool> GetToolById(Guid Id)
        {
            var item = await this.context.Tools
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

        public async Task<List<Tool>> GetTools()
        {
            return await this.context.Tools
                .AsNoTracking()
                .AsSplitQuery()
                .OrderByDescending(t => t.CreatedAt)
                .Include(t => t.ToolContents)
                    .ThenInclude(tl => tl.Language)
                .Include(t => t.Categories)
                    .ThenInclude(c => c.CategoryContents)
                        .ThenInclude(cc => cc.Language)
                .ToListAsync();
        }

        public async Task IncrementCommentCount(Guid Id)
        {
            await this.context.Tools
                .Where(p => p.Id == Id)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(
                        p => p.CommentCount,
                        p => p.CommentCount + 1
                    )
                );
        }

        public async Task IncrementLikeCount(Guid Id)
        {
            await this.context.Tools
                .Where(p => p.Id == Id)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(
                        p => p.LikeCount,
                        p => p.LikeCount + 1
                    )
                );
        }
        public async Task DecrementCommentCount(Guid Id)
        {
            await this.context.Tools
                .Where(p => p.Id == Id)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(
                        p => p.CommentCount,
                        p => p.CommentCount - 1
                    )
                );
        }

        public async Task DecrementLikeCount(Guid Id)
        {
            await this.context.Tools
                .Where(p => p.Id == Id)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(
                        p => p.LikeCount,
                        p => p.LikeCount - 1
                    )
                );
        }
    }
}