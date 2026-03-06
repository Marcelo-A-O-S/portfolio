using System.Linq.Expressions;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;

namespace PostService.Application.Services
{
    public class PostContentServices : IPostContentServices
    {
        private readonly IPostContentRepository postContentRepository;
        public PostContentServices(IPostContentRepository _postContentRepository)
        {
            this.postContentRepository = _postContentRepository;
        }
        public async Task Delete(PostContent entity)
        {
            await this.postContentRepository.Delete(entity);
        }

        public async Task<PostContent> FindBy(Expression<Func<PostContent, bool>> predicate)
        {
            return await this.postContentRepository.FindBy(predicate);
        }

        public async Task<PostContent> GetById(Guid Id)
        {
            return await this.postContentRepository.GetById(Id);
        }

        public async Task<List<PostContent>> List()
        {
            return await this.postContentRepository.List();
        }

        public async Task<List<PostContent>> List(int page)
        {
            return await this.postContentRepository.List(page);
        }

        public async Task Save(PostContent entity)
        {
            await this.postContentRepository.Save(entity);
        }

        public async Task Update(PostContent entity)
        {
            await this.postContentRepository.Update(entity);
        }
    }
}