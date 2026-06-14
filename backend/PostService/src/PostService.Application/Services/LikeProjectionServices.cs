using System.Linq.Expressions;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;

namespace PostService.Application.Services
{
    public class LikeProjectionServices : ILikeProjectionServices
    {
        private readonly ILikeProjectionServices likeProjectionServices;
        public LikeProjectionServices(
            ILikeProjectionServices _likeProjectionServices
        )
        {
            this.likeProjectionServices = _likeProjectionServices;
        }
        public async Task Delete(LikeProjection entity)
        {
            await this.likeProjectionServices.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.likeProjectionServices.DeleteById(Id);
        }

        public async Task<bool> Exists(Guid Id)
        {
            return await this.likeProjectionServices.Exists(Id);
        }

        public async Task<LikeProjection> FindBy(Expression<Func<LikeProjection, bool>> predicate)
        {
            return await this.likeProjectionServices.FindBy(predicate);
        }

        public async Task<LikeProjection> GetById(Guid Id)
        {
            return await this.likeProjectionServices.GetById(Id);
        }

        public async Task<List<LikeProjection>> List()
        {
            return await this.likeProjectionServices.List();
        }

        public async Task<List<LikeProjection>> List(int page)
        {
            return await this.likeProjectionServices.List(page);
        }

        public async Task Save(LikeProjection entity)
        {
            await this.likeProjectionServices.Save(entity);
        }

        public async Task Update(LikeProjection entity)
        {
            await this.likeProjectionServices.Update(entity);
        }
    }
}