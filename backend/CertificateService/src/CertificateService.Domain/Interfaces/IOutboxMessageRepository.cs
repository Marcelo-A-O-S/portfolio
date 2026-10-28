using CertificateService.Domain.Entities;
namespace CertificateService.Domain.Interfaces
{
    public interface IOutboxMessageRepository : IGenerics<OutboxMessage>
    {
        Task<List<OutboxMessage>> GetPendingMessages();
    }
}