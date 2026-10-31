using System.Linq.Expressions;
using CertificateService.Application.Interfaces;
using CertificateService.Domain.Entities;
using CertificateService.Domain.Interfaces;

namespace CertificateService.Application.Services
{
    public class PostContentProjectionServices : IPostContentProjectionServices
    {
        private readonly IPostContentProjectionRepository postContentProjectionRepository;
        public PostContentProjectionServices(
            IPostContentProjectionRepository _postContentProjectionRepository
        )
        {
            this.postContentProjectionRepository = _postContentProjectionRepository;
        }
        public async Task Delete(PostContentProjection entity)
        {
            await this.postContentProjectionRepository.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.postContentProjectionRepository.DeleteById(Id);
        }

        public async Task<bool> Exists(Guid Id)
        {
            return await this.postContentProjectionRepository.Exists(Id);
        }

        public async Task<PostContentProjection> FindBy(Expression<Func<PostContentProjection, bool>> predicate)
        {
            return await this.postContentProjectionRepository.FindBy(predicate);
        }

        public async Task<PostContentProjection> GetById(Guid Id)
        {
            return await this.postContentProjectionRepository.GetById(Id);
        }

        public async Task<List<PostContentProjection>> List()
        {
            return await this.postContentProjectionRepository.List();
        }

        public async Task<List<PostContentProjection>> List(int page)
        {
            return await this.postContentProjectionRepository.List(page);
        }

        public async Task Save(PostContentProjection entity)
        {
            await this.postContentProjectionRepository.Save(entity);
        }

        public async Task Update(PostContentProjection entity)
        {
            await this.postContentProjectionRepository.Update(entity);
        }
    }
}