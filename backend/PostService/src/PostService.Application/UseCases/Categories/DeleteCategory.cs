using PostService.Application.Exceptions;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Categories.Interfaces;
using PostService.Domain.Entities;

namespace PostService.Application.UseCases.Categories
{
    public class DeleteCategory : IDeleteCategory
    {
        private readonly ICategoryServices categoryServices;
        private readonly IUnitOfWork unitOfWork;
        public DeleteCategory(
            ICategoryServices _categoryServices,
            IUnitOfWork _unitOfWork
        )
        {
            this.categoryServices = _categoryServices;
            this.unitOfWork = _unitOfWork;
        }
        public async Task ExecuteAsync(Guid Id)
        {
            var category = await GetCategory(Id);
            await this.unitOfWork.BeginAsync();
            try
            {
                await this.categoryServices.Delete(category);
                await this.unitOfWork.CommitAsync();
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
        }
        private async Task<Category> GetCategory(Guid Id)
        {
            var category = await this.categoryServices.GetById(Id);
            if (category == null)
                throw new NotFoundException("Categoria não encontrada.");
            return category;
        }
    }
}