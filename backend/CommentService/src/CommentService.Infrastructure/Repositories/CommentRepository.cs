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
            var query = context.Comments
                .Include(c => c.UserProjection)
                .AsNoTracking()
                .Where(c =>
                    c.TargetId == targetId &&
                    c.Type == type);
            var totalItems = await query.CountAsync(c => c.ParentCommentId == null);
            var roots = await query
                .Where(c => c.ParentCommentId == null)
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * itemsPage)
                .Take(itemsPage)
                .ToListAsync();
            var replies = await query
                .Where(c => c.ParentCommentId != null)
                .ToListAsync();
            var all = roots
                .Concat(replies)
                .Select(c => new CommentView
                {
                    Id = c.Id,
                    ParentCommentId = c.ParentCommentId,
                    TargetId = c.TargetId,
                    Type = c.Type,
                    Content = c.Content,
                    Likes = this.context.Likes.Count(l =>
                                l.TargetId == c.Id),
                    Liked = this.context.Likes.Any(l =>
                                l.TargetId == c.Id &&
                                l.UserId == authenticatedUserId),
                    CreatedAt = c.CreatedAt,
                    User = new UserView
                    {
                        Id = c.UserProjection.UserId,
                        Username = c.UserProjection.Username,
                        ProfileUrl = c.UserProjection.ProfileUrl,
                        Provider = c.UserProjection.Provider
                    },
                    Replies = []
                })
                .ToList();
            var map = all.ToDictionary(c => c.Id!.Value);
            foreach (var comment in all)
            {
                if (comment.ParentCommentId is Guid parentId &&
                    map.TryGetValue(parentId, out var parent))
                {
                    parent.Replies.Add(comment);
                }
            }
            var items = all.Where(c => c.ParentCommentId == null).ToList();
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
                .OrderByDescending(p => p.CreatedAt)
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
        public async Task<Comment> GetFullDataById(Guid id)
        {
            return await this.context.Comments
                .Where(c => c.Id == id)
                .Include(c => c.UserProjection)
                .FirstOrDefaultAsync();
        }
    }
}