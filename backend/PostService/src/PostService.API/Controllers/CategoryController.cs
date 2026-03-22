using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using PostService.Application.DTOs.Request;

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
        [Authorize( Roles = "Administrador")]
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
        [HttpGet("{Id}")]
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
        [HttpPut("{Id}")]
        [Authorize(Roles = "Administrador")]
        [EnableRateLimiting("write")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid Id, CategoryRequest categoryRequest)
        {
            if (ModelState.IsValid)
            {
                var category = await this.categoryServices.GetById(Id);
                if (category == null)
                    return NotFound(new { message = "Categoria não encontrada." });
                foreach (var ccRequest in categoryRequest.CategoryContents)
                {
                    if (ccRequest.Id is not Guid categoryContentId)
                        return BadRequest(new { message = "O identificador relacionda ao conteudo da categoria é obrigatória." });
                    var categoryContent = await this.categoryContentServices.GetById(categoryContentId);
                    if (categoryContent == null)
                    {
                        categoryContent = new CategoryContent(category.Id, ccRequest.LanguageId, ccRequest.Name, ccRequest.Slug);
                    }
                    else
                    {
                        categoryContent.Update(ccRequest.LanguageId, ccRequest.Name, ccRequest.Slug);
                    }
                    category.AddCategoryContent(categoryContent);
                }
                await this.categoryServices.Update(category);
                return Ok(new { message = "Categoria atualizada com sucesso." });
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpDelete]
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