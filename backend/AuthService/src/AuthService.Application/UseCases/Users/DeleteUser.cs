using AuthService.Application.Exceptions;
using AuthService.Application.Interfaces;
using AuthService.Application.UseCases.Users.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using System.Text.Json;
namespace AuthService.Application.UseCases.Users
{
    public class DeleteUser : IDeleteUser
    {
        private readonly IUserServices userServices;
        private readonly IOutboxMessageServices outboxMessageServices;
        private readonly IUnitOfWork unitOfWork;
        public DeleteUser(
            IUserServices _userServices,
            IOutboxMessageServices _outboxMessageServices,
            IUnitOfWork _unitOfWork
        )
        {
            this.userServices = _userServices;
            this.outboxMessageServices = _outboxMessageServices;
            this.unitOfWork = _unitOfWork;
        }
        public async Task ExecuteAsync(Guid Id)
        {
            await this.unitOfWork.BeginAsync();
            try
            {
                var user = await GetUserByIdAsync(Id);
                await this.userServices.Delete(user);
                var outboxMessage = new OutboxMessage(
                    "UserDeleted",
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