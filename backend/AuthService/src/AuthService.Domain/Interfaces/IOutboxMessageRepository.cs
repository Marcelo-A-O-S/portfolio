using AuthService.Domain.Entities;
namespace AuthService.Domain.Interfaces
{
    public interface IOutboxMessageRepository : IGenerics<OutboxMessage>
    {
        Task<List<OutboxMessage>> GetPendingMessages();
        Task DeleteProcessedBefore(DateTime date);
    }
}