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
        private readonly IRabbitMQProducer rabbitMQProducer;
        public BanUser(
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
            user.Ban();
            await this.userServices.Update(user);
            await this.rabbitMQProducer.Publish("user-ban", new { UserId = Id });
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