using System.Linq.Expressions;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;

namespace PostService.Application.Services
{
    public class ToolContentServices : IToolContentServices
    {
        private readonly IToolContentRepository toolContentRepository;
        public ToolContentServices(IToolContentRepository _toolContentRepository)
        {
            this.toolContentRepository = _toolContentRepository;
        }

        public async Task Delete(ToolContent entity)
        {
            await this.toolContentRepository.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.toolContentRepository.DeleteById(Id);
        }

        public async Task<ToolContent> FindBy(Expression<Func<ToolContent, bool>> predicate)
        {
            return await this.toolContentRepository.FindBy(predicate);
        }

        public async Task<ToolContent> GetById(Guid Id)
        {
            return await this.toolContentRepository.GetById(Id);
        }

        public async Task<ToolContent> GetByToolId(Guid toolId)
        {
            return await this.toolContentRepository.GetByToolId(toolId);
        }

        public async Task<List<ToolContent>> List()
        {
            return await this.toolContentRepository.List();
        }

        public async Task<List<ToolContent>> List(int page)
        {
            return await this.toolContentRepository.List(page);
        }

        public async Task Save(ToolContent entity)
        {
            await this.toolContentRepository.Save(entity);
        }

        public async Task Update(ToolContent entity)
        {
            await this.toolContentRepository.Update(entity);
        }
    }
}