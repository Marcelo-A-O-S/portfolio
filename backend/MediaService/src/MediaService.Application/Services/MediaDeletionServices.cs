using System.Linq.Expressions;
using MediaService.Application.Interfaces;
using MediaService.Domain.Entities;
using MediaService.Domain.Interfaces;

namespace MediaService.Application.Services
{
    public class MediaDeletionServices : IMediaDeletionServices
    {
        private readonly IMediaDeletionRepository mediaDeletionRepository;
        public MediaDeletionServices(
            IMediaDeletionRepository _mediaDeletionRepository
        )
        {
            this.mediaDeletionRepository = _mediaDeletionRepository;
        }
        public async Task Delete(MediaDeletion entity)
        {
            await this.mediaDeletionRepository.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.mediaDeletionRepository.DeleteById(Id);
        }

        public async Task<bool> Exists(Guid Id)
        {
            return await this.mediaDeletionRepository.Exists(Id);
        }

        public async Task<MediaDeletion> FindBy(Expression<Func<MediaDeletion, bool>> predicate)
        {
            return await this.mediaDeletionRepository.FindBy(predicate);
        }

        public async Task<MediaDeletion> GetById(Guid Id)
        {
            return await this.mediaDeletionRepository.GetById(Id);
        }

        public async Task<List<MediaDeletion>> GetMediaPending()
        {
            return await this.mediaDeletionRepository.GetMediaPending();
        }

        public async Task<List<MediaDeletion>> List()
        {
            return await this.mediaDeletionRepository.List();
        }

        public async Task<List<MediaDeletion>> List(int page)
        {
            return await this.mediaDeletionRepository.List(page);
        }

        public async Task Save(MediaDeletion entity)
        {
            await this.mediaDeletionRepository.Save(entity);
        }

        public async Task Update(MediaDeletion entity)
        {
            await this.mediaDeletionRepository.Update(entity);
        }
    }
}