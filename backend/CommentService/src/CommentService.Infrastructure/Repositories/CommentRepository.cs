using CommentService.Domain.Entities;
using CommentService.Domain.Enums;
using CommentService.Domain.Interfaces;
using CommentService.Domain.Queries;
using CommentService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
namespace CommentService.Infrastructure.Repositories
{
    public class CommentRepository : Generics<Comment>, ICommentRepository
    {
        private readonly DBContext context;
        public CommentRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }

        public async Task<List<Comment>> GetCommentsByTargeIdsPage(List<Guid> targetIds)
        {
            return await this.context.Comments
                .Where(c => targetIds.Contains(c.TargetId))
                .ToListAsync();
        }
        public async Task<PaginatedResult<CommentView>> GetCommentsPaginationByTargetAndType(
            Guid? authenticatedUserId, Guid targetId, CommentType type, int page, int itemsPage = 10)
        {
            var query = this.context.Comments
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable();
            query = query
                .Where(c => c.TargetId == targetId && c.Type == type);
            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(c=> c.CreatedAt)
                .Include(c => c.ParentComment)
                .Include(c => c.Replies)
                .Skip((page - 1) * itemsPage)
                .Take(itemsPage)
                .Select(c => new CommentView
                {
                    Id = c.Id,
                    Content = c.Content,
                    Replies = c.Replies.Select(rp => new CommentView{
                        Id = rp.Id,
                        Content = rp.Content,
                        Replies = c.Replies.Select(rp => new CommentView{
                            Id = rp.Id,
                            Content = rp.Content,
                        }).ToList()
                    }).ToList()
                })
                .ToListAsync();
            return new PaginatedResult<CommentView>
            {
                Items = items,
                TotalItems = totalItems,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)itemsPage)
            };  
        }

        public async Task<List<Comment>> GetCommentsByTargetId(Guid targetId)
        {
            return await this.context.Comments
                .AsSplitQuery()
                .OrderByDescending(p=> p.CreatedAt)
                .Where(c => c.TargetId == targetId)
                .Include(c => c.ParentComment)
                .ToListAsync();
        }

        public async Task<Dictionary<Guid, int>> GetQuantityCommentsByTargeIdsPage(List<Guid> targetIds)
        {
            return await this.context.Comments
                .Where(c => targetIds.Contains(c.TargetId))
                .GroupBy(c => c.TargetId)
                .Select(g => new
                {
                    TargetId = g.Key,
                    Count = g.Count()
                })
                .ToDictionaryAsync(
                    x => x.TargetId,
                    x => x.Count
                );     
        }
    }
}