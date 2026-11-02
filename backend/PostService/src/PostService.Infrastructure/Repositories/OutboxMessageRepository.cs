using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
using PostService.Domain.Enums;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Context;

namespace PostService.Infrastructure.Repositories
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
                .Where(message => message.Status == OutboxStatus.Pending)
                .OrderBy(message => message.CreatedAt)
                .Take(50)
                .ToListAsync();
        }
        public async Task<List<OutboxMessage>> GetFailedMessages()
        {
            return await context.OutboxMessages
                .Where(message => message.Status == OutboxStatus.Failed)
                .OrderBy(message => message.CreatedAt)
                .Take(50)
                .ToListAsync();
        }
        public async Task DeleteFailedMessages()
        {
            await context.OutboxMessages
                .Where(message => message.Status == OutboxStatus.Failed)
                .ExecuteDeleteAsync();
        }
        public async Task DeleteFailedMessagesBefore(DateTime date)
        {
            await context.OutboxMessages
                .Where(message =>
                    message.Status == OutboxStatus.Failed &&
                    message.CreatedAt < date)
                .ExecuteDeleteAsync();
        }
    }
}