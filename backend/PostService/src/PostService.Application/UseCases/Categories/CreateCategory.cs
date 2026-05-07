using PostService.Application.DTOs.Request;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Categories.Interfaces;
using PostService.Application.Validations;
using PostService.Domain.Entities;
namespace PostService.Application.UseCases.Categories
{
    public class CreateCategory : ICreateCategory
    {
        private readonly ICategoryServices categoryServices;
        private readonly ICategoryContentServices categoryContentServices;
        public CreateCategory(
            ICategoryServices _categoryServices,
            ICategoryContentServices _categoryContentServices
        )
        {
            this.categoryServices = _categoryServices;
            this.categoryContentServices = _categoryContentServices;
        }
        public async Task ExecuteAsync(CategoryRequest categoryRequest)
        {
            ValidateCategoryRequest(categoryRequest);
            var category = new Category();
            await ProcessCategoryContents(category, categoryRequest.CategoryContents);
            await this.categoryServices.Save(category);
        }
        private static void ValidateCategoryRequest(CategoryRequest categoryRequest)
        {
            var validationError = ValidationHelper.Validate(categoryRequest);
            if (validationError.Count > 0)
                throw new Exception($"Erro ao validar dados: {validationError}");
        }
        private async Task ProcessCategoryContents(Category category, List<CategoryContentRequest> categoryContentRequests)
        {
            foreach (CategoryContentRequest? ccRequest in categoryContentRequests)
            {
                var validationError = ValidationHelper.Validate(ccRequest);
                if (validationError.Count > 0)
                    throw new Exception($"Erro ao validar dados: {validationError}");
                var categoryContent = await this.categoryContentServices.FindBy(cc => cc.Slug == ccRequest.Slug && cc.LanguageId == ccRequest.LanguageId);
                if (categoryContent != null)
                    throw new Exception("Erro ao validar dados!");
                categoryContent = new CategoryContent(category.Id, ccRequest.LanguageId, ccRequest.Name, ccRequest.Slug);
                category.AddCategoryContent(categoryContent);
            }
        }
    }
}