using System.Linq.Expressions;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;

namespace PostService.Application.Services
{
    public class LikeProjectionServices : ILikeProjectionServices
    {
        private readonly ILikeProjectionRepository likeProjectionRepository;
        public LikeProjectionServices(
            ILikeProjectionRepository _likeProjectionRepository
        )
        {
            this.likeProjectionRepository = _likeProjectionRepository;
        }
        public async Task Delete(LikeProjection entity)
        {
            await this.likeProjectionRepository.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.likeProjectionRepository.DeleteById(Id);
        }

        public async Task<bool> Exists(Guid Id)
        {
            return await this.likeProjectionRepository.Exists(Id);
        }

        public async Task<LikeProjection> FindBy(Expression<Func<LikeProjection, bool>> predicate)
        {
            return await this.likeProjectionRepository.FindBy(predicate);
        }

        public async Task<LikeProjection> GetById(Guid Id)
        {
            return await this.likeProjectionRepository.GetById(Id);
        }

        public async Task<List<LikeProjection>> List()
        {
            return await this.likeProjectionRepository.List();
        }

        public async Task<List<LikeProjection>> List(int page)
        {
            return await this.likeProjectionRepository.List(page);
        }

        public async Task Save(LikeProjection entity)
        {
            await this.likeProjectionRepository.Save(entity);
        }

        public async Task Update(LikeProjection entity)
        {
            await this.likeProjectionRepository.Update(entity);
        }
    }
}