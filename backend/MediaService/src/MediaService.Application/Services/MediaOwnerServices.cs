using System.Linq.Expressions;
using MediaService.Application.Interfaces;
using MediaService.Domain.Entities;
using MediaService.Domain.Interfaces;
namespace MediaService.Application.Services
{
    public class MediaOwnerServices : IMediaOwnerServices
    {
        private readonly IMediaOwnerRepository mediaOwnerRepository;
        public MediaOwnerServices(
            IMediaOwnerRepository _mediaOwnerRepository
        )
        {
            this.mediaOwnerRepository = _mediaOwnerRepository;
        }
        public async Task Delete(MediaOwner entity)
        {
            await this.mediaOwnerRepository.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.mediaOwnerRepository.DeleteById(Id);
        }

        public async Task<bool> Exists(Guid Id)
        {
            return await this.mediaOwnerRepository.Exists(Id);
        }

        public async Task<MediaOwner> FindBy(Expression<Func<MediaOwner, bool>> predicate)
        {
            return await this.mediaOwnerRepository.FindBy(predicate);
        }

        public async Task<MediaOwner> GetById(Guid Id)
        {
            return await this.mediaOwnerRepository.GetById(Id);
        }

        public async Task<List<MediaOwner>> List()
        {
            return await this.mediaOwnerRepository.List();
        }

        public async Task<List<MediaOwner>> List(int page)
        {
            return await this.mediaOwnerRepository.List(page);
        }

        public async Task Save(MediaOwner entity)
        {
            await this.mediaOwnerRepository.Save(entity);
        }

        public async Task Update(MediaOwner entity)
        {
            await this.mediaOwnerRepository.Update(entity);
        }
    }
}