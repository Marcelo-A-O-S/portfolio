using System.Linq.Expressions;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;

namespace PostService.Application.Services
{
    public class ToolsServices : IToolsServices
    {
        private readonly IToolsRepository toolsRepository;
        public ToolsServices(IToolsRepository _toolsRepository)
        {
            this.toolsRepository = _toolsRepository;
        }
        public async Task Delete(Tool entity)
        {
            await this.toolsRepository.Delete(entity);
        }

        public async Task<Tool> FindBy(Expression<Func<Tool, bool>> predicate)
        {
            return await this.toolsRepository.FindBy(predicate);
        }

        public async Task<Tool> GetById(Guid Id)
        {
            return await this.toolsRepository.GetById(Id);
        }

        public async Task<PaginatedResult<Tool>> GetByPagination(int page, string? search)
        {
            return await this.toolsRepository.GetByPagination(page,search);
        }

        public async Task<Tool> GetToolById(Guid Id)
        {
            return await this.toolsRepository.GetToolById(Id);
        }

        public async Task<List<Tool>> List()
        {
            return await this.toolsRepository.List();
        }

        public async Task<List<Tool>> List(int page)
        {
            return await this.toolsRepository.List(page);
        }

        public async Task Save(Tool entity)
        {
            await this.toolsRepository.Save(entity);
        }

        public async Task Update(Tool entity)
        {
            await this.toolsRepository.Update(entity);
        }
    }
}