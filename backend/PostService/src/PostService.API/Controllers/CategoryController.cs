using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        [Authorize( Roles = "Administrador")]
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
        [HttpGet]
        [Authorize( Roles = "Administrador")]
        public async Task<IActionResult> GetByPagination(
            [FromQuery] int page,
            [FromQuery] string? language,
            [FromQuery] string? search
        )
        {
            var categories = await this.categoryServices.GetByPagination(page, language, search);
            return Ok(categories);
        }
        [HttpGet("{Id}")]
        [Authorize( Roles = "Administrador")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            var category = await this.categoryServices.GetById(Id);
            if (category != null)
                return NotFound("Categoria não encontrada.");
            return Ok(category);
        }
        [HttpPost]
        [Authorize( Roles = "Administrador")]
        public async Task<IActionResult> CreateCategory(CategoryRequest categoryRequest)
        {
            if (ModelState.IsValid)
            {
                var category = new Category();
                var categoryContent = await this.categoryContentServices.FindBy(c => c.Slug == categoryRequest.Slug);
                if(categoryContent == null)
                {
                    return BadRequest(new { message = "Erro ao criar categoria."});
                }
                categoryContent.Update(category.Id, categoryRequest.Language, categoryRequest.Name, categoryRequest.Slug);
                await this.categoryServices.Save(category);
                await this.categoryContentServices.Save(categoryContent);
                return Ok(new { message = "Categoria salva com sucesso." });
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpPut("{Id}")]
        [Authorize( Roles = "Administrador")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid Id, CategoryRequest categoryRequest)
        {
            if (ModelState.IsValid)
            {
                var category = await this.categoryServices.GetById(Id);
                if (category == null)
                    return NotFound("Categoria não encontrada.");
                var categoryContent = await this.categoryContentServices.FindBy(c => c.Id == categoryRequest.Id &&  c.CategoryId == Id && c.Slug == categoryRequest.Slug && c.Language == categoryRequest.Language);
                if(categoryContent == null)
                    return NotFound("Categoria não encontrada.");
                categoryContent.Update(category.Id, categoryRequest.Language, categoryRequest.Name, categoryRequest.Slug);
                await this.categoryContentServices.Update(categoryContent);
                return Ok(new { message = "Categoria atualizada com sucesso." });
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpDelete]
        [Authorize( Roles = "Administrador")]
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