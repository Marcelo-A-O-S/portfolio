using System.Linq.Expressions;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;

namespace PostService.Application.Services
{
    public class PostToolServices : IPostToolServices
    {
        private readonly IPostToolsRepository postToolsRepository;
        public PostToolServices(IPostToolsRepository _postToolsRepository)
        {
            this.postToolsRepository = _postToolsRepository;
        }
        public async Task Delete(PostTool entity)
        {
            await this.postToolsRepository.Delete(entity);
        }

        public async Task<PostTool> FindBy(Expression<Func<PostTool, bool>> predicate)
        {
            return await this.postToolsRepository.FindBy(predicate);
        }

        public async Task<PostTool> GetById(Guid Id)
        {
            return await this.postToolsRepository.GetById(Id);
        }

        public async Task<List<PostTool>> List()
        {
            return await this.postToolsRepository.List();
        }

        public async Task<List<PostTool>> List(int page)
        {
            return await this.postToolsRepository.List(page);
        }

        public async Task Save(PostTool entity)
        {
            await this.postToolsRepository.Save(entity);
        }

        public async Task Update(PostTool entity)
        {
            await this.postToolsRepository.Update(entity);
        }
    }
}