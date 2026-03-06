using Microsoft.AspNetCore.Mvc;
using PostService.Application.DTOs.Request;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;

namespace PostService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryContentController : ControllerBase
    {
        private readonly ICategoryContentServices categoryContentServices;
        public CategoryContentController(
            ICategoryContentServices _categoryContentServices
        )
        {
            this.categoryContentServices = _categoryContentServices;
        }
        [HttpGet]
        public async Task<IActionResult> List([FromQuery]int? page)
        {
            var categoryContents = new List<CategoryContent>();
            if (page.HasValue)
            {
                categoryContents = await this.categoryContentServices.List(page ?? 1);
            }
            else
            {
                categoryContents = await this.categoryContentServices.List();
            }
            return Ok(categoryContents);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            var categoryContent = await this.categoryContentServices.GetById(Id);
            if(categoryContent == null)
                return NotFound("Conteúdo da categoria não encontrada.");
            return Ok(categoryContent);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategoryContent(CategoryRequest categoryRequest)
        {
            if (ModelState.IsValid)
            {
                
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
    }
}