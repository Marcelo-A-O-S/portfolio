using CertificateService.Domain.Entities;

namespace CertificateService.Application.Interfaces
{
    public interface IOutboxMessageServices : IServices<OutboxMessage>
    {
        Task<List<OutboxMessage>> GetPendingMessages();
    }
}