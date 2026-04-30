using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PostService.Application.DTOs.Request;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;

namespace PostService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices categoryServices;
        private readonly ICategoryContentServices categoryContentServices;
        public CategoryController(ICategoryServices _categoryServices, ICategoryContentServices _categoryContentServices)
        {
            this.categoryServices = _categoryServices;
            this.categoryContentServices = _categoryContentServices;
        }
        [HttpGet("GetCategories")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await this.categoryServices.GetCategories();
            return Ok(categories);
        }
        [HttpGet("GetCategoriesByLanguage")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetCategoriesByLanguage([FromQuery] string language)
        {
            var categories = await this.categoryServices.GetCategoriesByLanguage(language);
            return Ok(categories);
        }
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> List([FromQuery] int? page)
        {
            var categories = new List<Category>();
            if (page.HasValue)
            {
                categories = await this.categoryServices.List(page ?? 1);
            }
            else
            {
                categories = await this.categoryServices.List();
            }
            return Ok(categories);
        }
        [HttpGet("GetByPagination")]
        [Authorize(Roles = "Administrador")]
        [EnableRateLimiting("pagination")]
        public async Task<IActionResult> GetByPagination(
            [FromQuery] int page,
            [FromQuery] string? language,
            [FromQuery] string? search
        )
        {
            var result = await this.categoryServices.GetByPagination(page, language, search);
            return Ok(result);
        }
        [HttpGet("{Id:guid}")]
        [Authorize(Roles = "Administrador")]
        [EnableRateLimiting("read")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            var category = await this.categoryServices.GetById(Id);
            if (category != null)
                return NotFound(new { message = "Categoria não encontrada." });
            return Ok(category);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [EnableRateLimiting("write")]
        public async Task<IActionResult> CreateCategory(CategoryRequest categoryRequest)
        {
            if (ModelState.IsValid)
            {
                var category = new Category();
                if (categoryRequest.CategoryContents.Count == 0)
                    return BadRequest(new { message = "Não é possivel salvar uma categoria vazia" });
                foreach (var ccRequest in categoryRequest.CategoryContents)
                {
                    var categoryContent = await this.categoryContentServices.FindBy(cc => cc.Slug == ccRequest.Slug && cc.LanguageId == ccRequest.LanguageId);
                    if (categoryContent != null)
                        return BadRequest(new { message = "Erro ao validar dados!" });
                    categoryContent = new CategoryContent(category.Id, ccRequest.LanguageId, ccRequest.Name, ccRequest.Slug);
                    category.AddCategoryContent(categoryContent);
                }
                await this.categoryServices.Save(category);
                return Ok(new { message = "Categoria salva com sucesso." });
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpPut("{Id:guid}")]
        [Authorize(Roles = "Administrador")]
        [EnableRateLimiting("write")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid Id, CategoryRequest categoryRequest)
        {
            if (ModelState.IsValid)
            {
                var category = await this.categoryServices.GetForUpdate(Id);
                if (category == null)
                    return NotFound(new { message = "Categoria não encontrada." });
                var requestCategoryContentIds = categoryRequest.CategoryContents
                    .Where(cc => cc.Id.HasValue)
                    .Select(cc => cc.Id!.Value);
                category.ValidateCategoryContents(requestCategoryContentIds);
                foreach (var ccRequest in categoryRequest.CategoryContents)
                {
                    
                    if (ccRequest.Id.HasValue)
                    {
                        var categoryContent = category.CategoryContents.FirstOrDefault(cc => cc.Id == ccRequest.Id.Value);
                        if (categoryContent == null)
                            return NotFound(new { message = "Conteudo da categoria não encontrado."});
                        categoryContent.Update(ccRequest.LanguageId, ccRequest.Name, ccRequest.Slug);
                    }
                    else
                    {
                        var categoryContent = new CategoryContent(category.Id, ccRequest.LanguageId, ccRequest.Name, ccRequest.Slug);
                        category.AddCategoryContent(categoryContent);
                    }                        
                }
                await this.categoryServices.Update(category);
                return Ok(new { message = "Categoria atualizada com sucesso." });
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpDelete("{Id:guid}")]
        [Authorize(Roles = "Administrador")]
        [EnableRateLimiting("sensitive")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid Id)
        {
            var category = await this.categoryServices.GetById(Id);
            if (category == null)
                return NotFound("Categoria não encontrada.");
            await this.categoryServices.Delete(category);
            return Ok(new { message = "Categoria atualizada com sucesso." });
        }
    }
}