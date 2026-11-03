using CertificateService.Domain.Entities;

namespace CertificateService.Application.Interfaces
{
    public interface IOutboxMessageServices : IServices<OutboxMessage>
    {
        Task<List<OutboxMessage>> GetPendingMessages();
        Task DeleteProcessedBefore(DateTime date);
        Task<List<OutboxMessage>> GetFailedMessages();
        Task DeleteFailedMessages();
        Task DeleteFailedMessagesBefore(DateTime date);
        Task<List<OutboxMessage>> GetProcessingMessagesByTimeLimit(DateTime date);
    }
}