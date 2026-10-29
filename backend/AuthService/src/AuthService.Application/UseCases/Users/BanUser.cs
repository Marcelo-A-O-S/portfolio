using System.Text.Json;
using AuthService.Application.Exceptions;
using AuthService.Application.Interfaces;
using AuthService.Application.UseCases.Users.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;

namespace AuthService.Application.UseCases.Users
{
    public class BanUser : IBanUser
    {
        private readonly IUserServices userServices;
        private readonly IOutboxMessageServices outboxMessageServices;
        private readonly IUnitOfWork unitOfWork;
        private readonly IRabbitMQProducer rabbitMQProducer;
        public BanUser(
            IUserServices _userServices,
            IOutboxMessageServices _outboxMessageServices,
            IUnitOfWork _unitOfWork,
            IRabbitMQProducer _rabbitMQProducer
        )
        {
            this.userServices = _userServices;
            this.outboxMessageServices = _outboxMessageServices;
            this.unitOfWork = _unitOfWork;
            this.rabbitMQProducer = _rabbitMQProducer;
        }
        public async Task ExecuteAsync(Guid Id)
        {
            await this.unitOfWork.BeginAsync();
            try
            {
                var user = await GetUserByIdAsync(Id);
                user.Ban();
                await this.userServices.Update(user);
                var outboxMessage = new OutboxMessage(
                    "UserBaned",
                    JsonSerializer.Serialize(new
                    {
                        UserId = Id
                    })
                );
                await this.outboxMessageServices.Save(outboxMessage);
                await this.unitOfWork.CommitAsync();
            }
            catch
            {
                await this.unitOfWork.RollbackAsync();
                throw;
            }
        }
        private async Task<User> GetUserByIdAsync(Guid Id)
        {
            var user = await this.userServices.GetById(Id);
            if (user == null)
                throw new NotFoundException("Usuário não encontrado");
            return user;
        }
    }
}