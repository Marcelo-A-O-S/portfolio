using AuthService.Domain.Entities;
namespace AuthService.Application.Interfaces
{
    public interface IOutboxMessageServices : IServices<OutboxMessage>
    {
        Task<List<OutboxMessage>> GetPendingMessages();
        Task DeleteProcessedBefore(DateTime date);
    }
}