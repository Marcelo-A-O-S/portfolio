using CertificateService.Domain.Entities;
using CertificateService.Domain.Interfaces;
using CertificateService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
namespace CertificateService.Infrastructure.Repositories
{
    public class OutboxMessageRepository : Generics<OutboxMessage>, IOutboxMessageRepository
    {
        private readonly DBContext context;
        public OutboxMessageRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
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