using MediaService.Domain.Entities;

namespace MediaService.Application.Interfaces
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