using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PostService.Application.DTOs.Request;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Categories.Interfaces;
using PostService.Domain.Entities;

namespace PostService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices categoryServices;
        private readonly ICategoryContentServices categoryContentServices;
        private readonly ICreateCategory createCategory;
        private readonly IUpdateCategory updateCategory;
        private readonly IDeleteCategory deleteCategory;
        public CategoryController(
            ICategoryServices _categoryServices, 
            ICategoryContentServices _categoryContentServices,
            ICreateCategory _createCategory,
            IUpdateCategory _updateCategory,
            IDeleteCategory _deleteCategory)
        {
            this.categoryServices = _categoryServices;
            this.categoryContentServices = _categoryContentServices;
            this.createCategory = _createCategory;
            this.updateCategory = _updateCategory;
            this.deleteCategory = _deleteCategory;
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
                await this.createCategory.ExecuteAsync(categoryRequest);
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
                await this.updateCategory.ExecuteAsync(Id, categoryRequest);
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
            await this.deleteCategory.ExecuteAsync(Id);
            return Ok(new { message = "Categoria atualizada com sucesso." });
        }
    }
}