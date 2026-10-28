using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;

namespace PostService.Infrastructure.Repositories
{
    public class OutboxMessageRepository : Generics<OutboxMessage>, IOutboxMessageRepository
    {
        private readonly DBContext context;
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
                .Where(message => message.ProcessedAt == null)
                .OrderBy(message => message.CreatedAt)
                .Take(50)
                .ToListAsync();
        }
    }
}