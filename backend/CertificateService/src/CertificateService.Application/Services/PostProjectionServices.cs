using System.Linq.Expressions;
using CertificateService.Application.Interfaces;
using CertificateService.Domain.Entities;
using CertificateService.Domain.Interfaces;

namespace CertificateService.Application.Services
{
    public class PostProjectionServices : IPostProjectionServices
    {
        private readonly IPostProjectionRepository postProjectionRepository;
        public PostProjectionServices(
            IPostProjectionRepository _postProjectionRepository
        )
        {
            this.postProjectionRepository = _postProjectionRepository;
        }
        public async Task Delete(PostProjection entity)
        {
            await this.postProjectionRepository.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.postProjectionRepository.DeleteById(Id);
        }

        public async Task<bool> Exists(Guid Id)
        {
            return await this.postProjectionRepository.Exists(Id);
        }

        public async Task<PostProjection> FindBy(Expression<Func<PostProjection, bool>> predicate)
        {
            return await this.postProjectionRepository.FindBy(predicate);
        }

        public async Task<PostProjection> GetById(Guid Id)
        {
            return await this.postProjectionRepository.GetById(Id);
        }

        public async Task<List<PostProjection>> List()
        {
            return await this.postProjectionRepository.List();
        }

        public async Task<List<PostProjection>> List(int page)
        {
            return await this.postProjectionRepository.List(page);
        }

        public async Task Save(PostProjection entity)
        {
            await this.postProjectionRepository.Save(entity);
        }

        public async Task Update(PostProjection entity)
        {
            await this.postProjectionRepository.Update(entity);
        }
    }
}