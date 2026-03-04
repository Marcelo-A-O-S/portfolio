using System.Linq.Expressions;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;

namespace PostService.Application.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRepository categoryRepository;
        public CategoryServices(ICategoryRepository _categoryRepository)
        {
            this.categoryRepository = _categoryRepository;
        }
        public async Task Delete(Category entity)
        {
            await this.categoryRepository.Delete(entity);
        }

        public async Task<Category> FindBy(Expression<Func<Category, bool>> predicate)
        {
            return await this.categoryRepository.FindBy(predicate);
        }

        public async Task<Category> GetById(Guid Id)
        {
            return await this.categoryRepository.GetById(Id);
        }

        public async Task<List<Category>> List()
        {
            return await this.categoryRepository.List();
        }

        public async Task<List<Category>> List(int page)
        {
            return await this.categoryRepository.List(page);
        }

        public async Task Save(Category entity)
        {
            await this.categoryRepository.Save(entity);
        }

        public async Task Update(Category entity)
        {
            await this.categoryRepository.Update(entity);
        }
    }
}