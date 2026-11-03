using PostService.Domain.Entities;

namespace PostService.Domain.Interfaces
{
    public interface IOutboxMessageRepository : IGenerics<OutboxMessage>
    {
        Task<List<OutboxMessage>> GetPendingMessages();
        Task DeleteProcessedBefore(DateTime date);
        Task<List<OutboxMessage>> GetFailedMessages();
        Task DeleteFailedMessages();
        Task DeleteFailedMessagesBefore(DateTime date);
        Task<List<OutboxMessage>> GetProcessingMessagesByTimeLimit(DateTime date);
    }
}