using System.Linq.Expressions;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;

namespace AuthService.Application.Services
{
    public class UserServices : IUserServices
    {
        public readonly IUserRepository userRepository;
        public UserServices(IUserRepository _userRepository)
        {
            this.userRepository = _userRepository;
        }
        public async Task Delete(User entity)
        {
            await this.userRepository.Delete(entity);
        }

        public async Task<User> FindBy(Expression<Func<User, bool>> predicate)
        {
            return await this.userRepository.FindBy(predicate);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await this.userRepository.GetUserByEmail(email);
        }

        public async Task<User> GetById(Guid Id)
        {
            return await this.userRepository.GetById(Id);
        }

        public async Task<List<User>> List()
        {
            return await this.userRepository.List();
        }

        public async Task<List<User>> List(int page)
        {
            return await this.userRepository.List(page);
        }

        public async Task Save(User entity)
        {
            await this.userRepository.Save(entity);
        }

        public async Task Update(User entity)
        {
            await this.userRepository.Update(entity);
        }

        public async Task<PaginatedResult<User>> GetByPagination(int page, string? search, string? role, string? status)
        {
            return await this.userRepository.GetByPagination(page, search, role, status);
        }
    }
}