using System.Linq.Expressions;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;

namespace PostService.Application.Services
{
    public class PostServices : IPostServices
    {
        private readonly IPostRepository postRepository;
        public PostServices(IPostRepository _postRepository)
        {
            this.postRepository = _postRepository;
        }
        public async Task Delete(Post entity)
        {
            await this.postRepository.Delete(entity);
        }

        public async Task<Post> FindBy(Expression<Func<Post, bool>> predicate)
        {
            return await this.postRepository.FindBy(predicate);
        }

        public async Task<Post> GetById(Guid Id)
        {
            return await this.postRepository.GetById(Id);
        }

        public async Task<PaginatedResult<Post>> GetByPagination(int page, string? search)
        {
            return await this.postRepository.GetByPagination(page, search);
        }

        public async Task<Post> GetForUpdate(Guid Id)
        {
            return await this.postRepository.GetForUpdate(Id);
        }

        public async Task<Post> GetPostById(Guid Id)
        {
            return await this.postRepository.GetPostById(Id);
        }

        public async Task<List<Post>> List()
        {
            return await this.postRepository.List();
        }

        public async Task<List<Post>> List(int page)
        {
            return await this.postRepository.List(page);
        }

        public async Task Save(Post entity)
        {
            await this.postRepository.Save(entity);
        }

        public async Task Update(Post entity)
        {
            await this.postRepository.Update(entity);
        }
    }
}