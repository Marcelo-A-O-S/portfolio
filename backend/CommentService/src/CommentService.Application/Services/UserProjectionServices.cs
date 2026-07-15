using System.Linq.Expressions;
using CommentService.Application.Interfaces;
using CommentService.Domain.Entities;
using CommentService.Domain.Interfaces;

namespace CommentService.Application.Services
{
    public class UserProjectionServices : IUserProjectionServices
    {
        private readonly IUserProjectionRepository userProjectionRepository;
        public UserProjectionServices(
            IUserProjectionRepository _userProjectionRepository
        )
        {
            this.userProjectionRepository = _userProjectionRepository;
        }
        public async Task Delete(UserProjection entity)
        {
            await this.userProjectionRepository.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.userProjectionRepository.DeleteById(Id);
        }

        public async Task<bool> Exists(Guid Id)
        {
            return await this.userProjectionRepository.Exists(Id);
        }

        public async Task<UserProjection> FindBy(Expression<Func<UserProjection, bool>> predicate)
        {
            return await this.userProjectionRepository.FindBy(predicate);
        }

        public async Task<UserProjection> GetById(Guid Id)
        {
            return await this.userProjectionRepository.GetById(Id);
        }

        public async Task<List<UserProjection>> List()
        {
            return await this.userProjectionRepository.List();
        }

        public async Task<List<UserProjection>> List(int page)
        {
            return await this.userProjectionRepository.List(page);
        }

        public async Task Save(UserProjection entity)
        {
            await this.userProjectionRepository.Save(entity);
        }

        public async Task Update(UserProjection entity)
        {
            await this.userProjectionRepository.Update(entity);
        }
    }
}