using System.Linq.Expressions;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;

namespace PostService.Application.Services
{
    public class MediaFileServices : IMediaFileServices
    {
        private readonly IMediaFileRepository repository;
        public MediaFileServices(IMediaFileRepository _repository)
        {
            this.repository = _repository;
        }
        public async Task Delete(MediaFile entity)
        {
            await this.repository.Delete(entity);
        }
        public async Task<MediaFile> FindBy(Expression<Func<MediaFile, bool>> predicate)
        {
            return await this.repository.FindBy(predicate);
        }

        public async Task<MediaFile> GetById(Guid Id)
        {
            return await this.repository.GetById(Id);
        }

        public async Task<List<MediaFile>> List()
        {
            return await this.repository.List();
        }

        public async Task<List<MediaFile>> List(int page)
        {
            return await this.repository.List(page);
        }

        public async Task Save(MediaFile entity)
        {
            await this.repository.Save(entity);
        }

        public async Task Update(MediaFile entity)
        {
            await this.repository.Update(entity);
        }
    }
}