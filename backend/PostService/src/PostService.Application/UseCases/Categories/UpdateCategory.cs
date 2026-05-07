using PostService.Application.DTOs.Request;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Categories.Interfaces;
using PostService.Application.Validations;
using PostService.Domain.Entities;

namespace PostService.Application.UseCases.Categories
{
    public class UpdateCategory : IUpdateCategory
    {
        private readonly ICategoryServices categoryServices;
        private readonly ICategoryContentServices categoryContentServices;
        public UpdateCategory(
            ICategoryServices _categoryServices,
            ICategoryContentServices _categoryContentServices
        )
        {
            this.categoryServices = _categoryServices;
            this.categoryContentServices = _categoryContentServices;
        }
        public async Task ExecuteAsync(Guid Id, CategoryRequest categoryRequest)
        {
            ValidateCategoryRequest(categoryRequest);
            var category = await this.categoryServices.GetForUpdate(Id);
            await ProcessCategories(category, categoryRequest.CategoryContents);
            await this.categoryServices.Update(category);
        }
        private static void ValidateCategoryRequest(CategoryRequest categoryRequest)
        {
            var validationError = ValidationHelper.Validate(categoryRequest);
            if (validationError.Count > 0)
                throw new Exception($"Erro ao validar dados: {validationError}");
        }
        private static async Task ProcessCategories(Category category, List<CategoryContentRequest> categoryContentRequests)
        {
            var requestCategoryContentIds = categoryContentRequests
                    .Where(cc => cc.Id.HasValue)
                    .Select(cc => cc.Id!.Value);
            category.ValidateCategoryContents(requestCategoryContentIds);
            foreach (var ccRequest in categoryContentRequests)
            {
                var validationError = ValidationHelper.Validate(ccRequest);
                if (validationError.Count > 0)
                    throw new Exception($"Erro ao validar dados: {validationError}");
                if (ccRequest.Id.HasValue)
                {
                    var categoryContent = category.CategoryContents.FirstOrDefault(cc => cc.Id == ccRequest.Id.Value);
                    if (categoryContent == null)
                        throw new Exception("Conteudo da categoria não encontrado.");
                    categoryContent.Update(ccRequest.LanguageId, ccRequest.Name, ccRequest.Slug);
                }
                else
                {
                    var categoryContent = new CategoryContent(category.Id, ccRequest.LanguageId, ccRequest.Name, ccRequest.Slug);
                    category.AddCategoryContent(categoryContent);
                }
            }
        }
    }
}