using System.Linq.Expressions;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;

namespace PostService.Application.Services
{
    public class MediaProjectionServices : IMediaProjectionServices
    {
        private readonly IMediaProjectionRepository mediaProjectionRepository;
        public MediaProjectionServices(
            IMediaProjectionRepository _mediaProjectionRepository
        )
        {
            this.mediaProjectionRepository = _mediaProjectionRepository;
        }
        public async Task Delete(MediaProjection entity)
        {
            await this.mediaProjectionRepository.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.mediaProjectionRepository.DeleteById(Id);
        }

        public async Task<bool> Exists(Guid Id)
        {
            return await this.mediaProjectionRepository.Exists(Id);
        }

        public async Task<MediaProjection> FindBy(Expression<Func<MediaProjection, bool>> predicate)
        {
            return await this.mediaProjectionRepository.FindBy(predicate);
        }

        public async Task<MediaProjection> GetById(Guid Id)
        {
            return await this.mediaProjectionRepository.GetById(Id);
        }

        public async Task<List<MediaProjection>> List()
        {
            return await this.mediaProjectionRepository.List();
        }

        public async Task<List<MediaProjection>> List(int page)
        {
            return await this.mediaProjectionRepository.List(page);
        }

        public async Task Save(MediaProjection entity)
        {
            await this.mediaProjectionRepository.Save(entity);
        }

        public async Task Update(MediaProjection entity)
        {
            await this.mediaProjectionRepository.Update(entity);
        }
    }
}