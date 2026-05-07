using PostService.Application.Interfaces;
using PostService.Application.UseCases.Categories.Interfaces;
using PostService.Domain.Entities;

namespace PostService.Application.UseCases.Categories
{
    public class DeleteCategory : IDeleteCategory
    {
        private readonly ICategoryServices categoryServices;
        public DeleteCategory(
            ICategoryServices _categoryServices
        )
        {
            this.categoryServices = _categoryServices;
        }
        public async Task ExecuteAsync(Guid Id)
        {
            var category = await GetCategory(Id);
            await this.categoryServices.Delete(category);
        }
        private async Task<Category> GetCategory(Guid Id)
        {
            var category = await this.categoryServices.GetById(Id);
            if (category == null)
                throw new Exception("Categoria não encontrada.");
            return category;
        }
    }
}