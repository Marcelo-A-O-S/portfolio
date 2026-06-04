using AuthService.Application.Exceptions;
using AuthService.Application.Interfaces;
using AuthService.Application.UseCases.Users.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AuthService.Application.UseCases.Users
{
    public class DeleteUser : IDeleteUser
    {
        private readonly IUserServices userServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        public DeleteUser(
            IUserServices _userServices,
            IRabbitMQProducer _rabbitMQProducer
        )
        {
            this.userServices = _userServices;
            this.rabbitMQProducer = _rabbitMQProducer;
        }
        public async Task ExecuteAsync(Guid Id)
        {
            var user = await GetUserByIdAsync(Id);
            await this.userServices.Delete(user);
            await this.rabbitMQProducer.Publish("delete-user", new { UserId = Id });
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