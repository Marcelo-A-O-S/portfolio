using System.Linq.Expressions;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;

namespace PostService.Application.Services
{
    public class PostCategoryServices : IPostCategoryServices
    {
        private readonly IPostCategoryRepository postCategoryRepository;
        public PostCategoryServices(IPostCategoryRepository _postCategoryRepository)
        {
            this.postCategoryRepository = _postCategoryRepository;
        }
        public async Task Delete(PostCategory entity)
        {
            await this.postCategoryRepository.Delete(entity);
        }

        public async Task<PostCategory> FindBy(Expression<Func<PostCategory, bool>> predicate)
        {
            return await this.postCategoryRepository.FindBy(predicate);
        }

        public async Task<PostCategory> GetById(Guid Id)
        {
            return await this.postCategoryRepository.GetById(Id);
        }

        public async Task<List<PostCategory>> List()
        {
            return await this.postCategoryRepository.List();
        }

        public async Task<List<PostCategory>> List(int page)
        {
            return await this.postCategoryRepository.List(page);
        }

        public async Task Save(PostCategory entity)
        {
            await this.postCategoryRepository.Save(entity);
        }

        public async Task Update(PostCategory entity)
        {
            await this.postCategoryRepository.Update(entity);
        }
    }
}