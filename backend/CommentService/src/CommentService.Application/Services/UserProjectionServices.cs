using System.Linq.Expressions;
using CommentService.Application.Interfaces;
using CommentService.Domain.Entities;

namespace CommentService.Application.Services
{
    public class UserProjectionServices : IUserProjectionServices
    {
        private readonly IUserProjectionServices userProjectionServices;
        public UserProjectionServices(
            IUserProjectionServices _userProjectionServices
        )
        {
            this.userProjectionServices = _userProjectionServices;
        }
        public async Task Delete(UserProjection entity)
        {
            await this.userProjectionServices.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.userProjectionServices.DeleteById(Id);
        }

        public async Task<bool> Exists(Guid Id)
        {
            return await this.userProjectionServices.Exists(Id);
        }

        public async Task<UserProjection> FindBy(Expression<Func<UserProjection, bool>> predicate)
        {
            return await this.userProjectionServices.FindBy(predicate);
        }

        public async Task<UserProjection> GetById(Guid Id)
        {
            return await this.userProjectionServices.GetById(Id);
        }

        public async Task<List<UserProjection>> List()
        {
            return await this.userProjectionServices.List();
        }

        public async Task<List<UserProjection>> List(int page)
        {
            return await this.userProjectionServices.List(page);
        }

        public async Task Save(UserProjection entity)
        {
            await this.userProjectionServices.Save(entity);
        }

        public async Task Update(UserProjection entity)
        {
            await this.userProjectionServices.Update(entity);
        }
    }
}