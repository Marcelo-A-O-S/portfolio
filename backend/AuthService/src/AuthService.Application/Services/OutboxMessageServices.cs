using System.Linq.Expressions;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;

namespace AuthService.Application.Services
{
    public class OutboxMessageServices : IOutboxMessageServices
    {
        private readonly IOutboxMessageRepository outboxMessageRepository;
        public OutboxMessageServices(
            IOutboxMessageRepository _outboxMessageRepository
        )
        {
            this.outboxMessageRepository = _outboxMessageRepository;
        }
        public async Task Delete(OutboxMessage entity)
        {
            await this.outboxMessageRepository.Delete(entity);
        }

        public async Task DeleteProcessedBefore(DateTime date)
        {
            await this.outboxMessageRepository.DeleteProcessedBefore(date);
        }

        public async Task<bool> Exists(Guid Id)
        {
            return await this.outboxMessageRepository.Exists(Id);
        }

        public async Task<OutboxMessage> FindBy(Expression<Func<OutboxMessage, bool>> predicate)
        {
            return await this.outboxMessageRepository.FindBy(predicate);
        }

        public async Task<OutboxMessage> GetById(Guid Id)
        {
            return await this.outboxMessageRepository.GetById(Id);
        }

        public async Task<List<OutboxMessage>> GetPendingMessages()
        {
            return await this.outboxMessageRepository.GetPendingMessages();
        }

        public async Task<List<OutboxMessage>> List()
        {
            return await this.outboxMessageRepository.List();
        }

        public async Task<List<OutboxMessage>> List(int page)
        {
            return await this.outboxMessageRepository.List(page);
        }

        public async Task Save(OutboxMessage entity)
        {
            await this.outboxMessageRepository.Save(entity);
        }

        public async Task Update(OutboxMessage entity)
        {
            await this.outboxMessageRepository.Update(entity);
        }
    }
}