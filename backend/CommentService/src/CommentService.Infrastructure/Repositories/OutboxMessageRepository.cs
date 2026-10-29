using CommentService.Domain.Entities;
using CommentService.Domain.Interfaces;
using CommentService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
namespace CommentService.Infrastructure.Repositories
{
    public class OutboxMessageRepository : Generics<OutboxMessage>, IOutboxMessageRepository
    {
        private readonly DBContext context;
        private const int MaxAttempts = 10;
        public OutboxMessageRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }
        public async Task DeleteProcessedBefore(DateTime date)
        {
            await context.OutboxMessages
                .Where(message =>
                    message.ProcessedAt != null &&
                    message.ProcessedAt < date)
                .ExecuteDeleteAsync();
        }
        public async Task<List<OutboxMessage>> GetPendingMessages()
        {
            return await this.context.OutboxMessages
                .Where(message => message.ProcessedAt == null && message.Attempts < MaxAttempts)
                .OrderBy(message => message.CreatedAt)
                .Take(50)
                .ToListAsync();
        }
    }
}