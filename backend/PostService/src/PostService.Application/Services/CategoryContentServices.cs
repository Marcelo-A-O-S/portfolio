using System.Linq.Expressions;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;

namespace PostService.Application.Services
{
    public class CategoryContentServices : ICategoryContentServices
    {
        private readonly ICategoryContentRepository categoryContentRepository;
        public CategoryContentServices(ICategoryContentRepository _categoryContentRepository)
        {
            this.categoryContentRepository = _categoryContentRepository;
        }
        public async Task Delete(CategoryContent entity)
        {
            await this.categoryContentRepository.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.categoryContentRepository.DeleteById(Id);
        }

        public async Task<CategoryContent> FindBy(Expression<Func<CategoryContent, bool>> predicate)
        {
            return await this.categoryContentRepository.FindBy(predicate);
        }

        public async Task<CategoryContent> GetById(Guid Id)
        {
            return await this.categoryContentRepository.GetById(Id);
        }

        public async Task<List<CategoryContent>> List()
        {
            return await this.categoryContentRepository.List();
        }

        public async Task<List<CategoryContent>> List(int page)
        {
            return await this.categoryContentRepository.List(page);
        }

        public async Task Save(CategoryContent entity)
        {
            await this.categoryContentRepository.Save(entity);
        }

        public async Task Update(CategoryContent entity)
        {
            await this.categoryContentRepository.Update(entity);
        }
    }
}