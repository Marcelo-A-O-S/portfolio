using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Domain.Queries;
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

        public async Task<PaginatedResult<PostView>> GetByPagination(Guid authenticatedUserId, int page, string? search, int itemsPage = 10)
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
                .OrderByDescending(p => p.CreatedAt)
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
                .Select(p => new PostView
                {
                    Id = p.Id,
                    ImgUrl = p.ImgUrl,
                    Status = p.Status,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    PostContents = p.PostContents.Select(pc => new PostContentView
                    {
                        Id = pc.Id,
                        Title = pc.Title,
                        Description = pc.Description,
                        Content = pc.Content,
                        Slug = pc.Slug,
                        ImagesUrls = pc.ImagesUrls,
                        CreatedAt = pc.CreatedAt,
                        Language = new LanguageView
                        {
                            Id = pc.Language.Id,
                            Code = pc.Language.Code,
                            Name = pc.Language.Name
                        }
                    }).ToList(),
                    Likes = p.LikeCount,
                    Liked = this.context.LikeProjections.Any(lp =>
                                lp.TargetId == p.Id &&
                                lp.UserId == authenticatedUserId),
                    Tools = p.Tools.Select(t => new ToolView
                    {
                        Id = t.Id,
                        ToolContents = t.ToolContents.Select(tc => new ToolContentView
                        {
                            Id = tc.Id,
                            Name = tc.Name,
                            Title = tc.Title,
                            Description = tc.Description,
                            Content = tc.Content,
                            Slug = tc.Slug,
                            ImagesUrls = tc.ImagesUrls,
                            CreatedAt = tc.CreatedAt,
                            Language = new LanguageView
                            {
                                Id = tc.Language.Id,
                                Code = tc.Language.Code,
                                Name = tc.Language.Name
                            }
                        }).ToList()
                    }).ToList(),
                    Categories = p.Categories.Select(c => new CategoryView
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
                })
                .ToListAsync();
            return new PaginatedResult<PostView>
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
                .AsSplitQuery()
                .Include(p => p.PostContents)
                .Include(p => p.Categories)
                .Include(p => p.Tools)
                .FirstOrDefaultAsync(t => t.Id == Id);
        }
        public async Task<Post> GetFullDataById(Guid Id)
        {
            return await context.Posts
                .AsSplitQuery()
                .Include(p => p.PostContents)
                .Include(p => p.Categories)
                .Include(p => p.Tools)
                .FirstOrDefaultAsync(t => t.Id == Id);
        }
        public async Task<int> GetLikesCountByPostId(Guid postId)
        {
            return await this.context.Posts
                .Where(p => p.Id == postId)
                .CountAsync();
        }
        public async Task<Post> GetPostById(Guid Id)
        {
            var item = await this.context.Posts
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
                .OrderByDescending(p => p.CreatedAt)
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

        public async Task IncrementCommentCount(Guid postId)
        {
            await this.context.Posts
                .Where(p => p.Id == postId)
                .ExecuteUpdateAsync(setters => 
                    setters.SetProperty(
                        p => p.CommentCount,
                        p => p.CommentCount + 1
                    )
                );
        }

        public async Task IncrementLikeCount(Guid postId)
        {
            await this.context.Posts
                .Where(p => p.Id == postId)
                .ExecuteUpdateAsync(setters => 
                    setters.SetProperty(
                        p => p.LikeCount,
                        p => p.LikeCount + 1
                    )
                );
        }
        public async Task DecrementCommentCount(Guid postId)
        {
            await this.context.Posts
                .Where(p => p.Id == postId)
                .ExecuteUpdateAsync(setters => 
                    setters.SetProperty(
                        p => p.CommentCount,
                        p => p.CommentCount - 1
                    )
                );
        }

        public async Task DecrementLikeCount(Guid postId)
        {
            await this.context.Posts
                .Where(p => p.Id == postId)
                .ExecuteUpdateAsync(setters => 
                    setters.SetProperty(
                        p => p.LikeCount,
                        p => p.LikeCount - 1
                    )
                );
        }
    }
}