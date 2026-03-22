using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostService.Application.DTOs.Request;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using PostService.Domain.Enums;

namespace PostService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToolController : ControllerBase
    {
        private readonly IToolsServices toolsServices;
        private readonly IToolContentServices toolContentServices;
        private readonly ICategoryServices categoryServices;
        public ToolController(
            IToolsServices _toolsServices,
            IToolContentServices _toolContentServices,
            ICategoryServices _categoryServices)
        {
            this.toolsServices = _toolsServices;
            this.toolContentServices = _toolContentServices;
            this.categoryServices = _categoryServices;
        }
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> List([FromQuery] int? page)
        {
            var tools = new List<Tool>();
            if (page.HasValue)
            {
                tools = await this.toolsServices.List(page ?? 1);
            }
            else
            {
                tools = await this.toolsServices.List();
            }
            return Ok(tools);
        }
        [HttpGet("{Id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            var tool = await this.toolsServices.GetById(Id);
            if (tool == null)
                return NotFound();
            return Ok(tool);
        }
        [HttpGet("GetToolById/{Id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetToolById([FromRoute] Guid Id)
        {
            var tool = await this.toolsServices.GetToolById(Id);
            if (tool == null)
                return NotFound();
            return Ok(tool);
        }
        [HttpGet("GetByPagination")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetByPagination(
            [FromQuery] int page,
            [FromQuery] string? search
        )
        {
            var result = await this.toolsServices.GetByPagination(page, search);
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CreateTool(ToolRequest toolRequest)
        {
            if (ModelState.IsValid)
            {
                if (toolRequest.toolContents.Count == 0)
                    return BadRequest("Não é possível salvar uma ferramenta sem seu conteudo relacionado.");
                if (toolRequest.Categories.Count == 0)
                    return BadRequest("Não é possível salvar uma ferramenta sem suas categorias relacionadas.");
                var tool = new Tool(toolRequest.ImgUrl);
                foreach (var item in toolRequest.toolContents)
                {
                    var toolContent = await this.toolContentServices.FindBy(tc => tc.Slug == item.Slug && tc.LanguageId == item.LanguageId);
                    if (toolContent != null)
                        return BadRequest(new { message = "Erro ao validar dados!" });
                    toolContent = new ToolContent(tool.Id, item.LanguageId, item.Name, item.Description, item.Content, item.Slug);
                    tool.AddToolContent(toolContent);
                }
                foreach (var item in toolRequest.Categories)
                {
                    if (item.Id is not Guid categoryId)
                        return BadRequest(new { message = "O identificador relacionado as categorias são obrigatórios" });
                    var category = await this.categoryServices.GetById(categoryId);
                    if (category == null)
                        return NotFound(new { message = "Categoria não encontrada." });
                    tool.AddCategory(category);
                }
                await this.toolsServices.Save(tool);
                return Ok(new { message = "Ferramenta salva com sucesso." });
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpPut("{Id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> UpdateTool([FromRoute] Guid Id, ToolRequest toolRequest)
        {
            if (ModelState.IsValid)
            {
                if (toolRequest.toolContents.Count == 0)
                    return BadRequest("Não é possível atualizar uma ferramenta sem seu conteudo relacionado.");
                if (toolRequest.Categories.Count == 0)
                    return BadRequest("Não é possível atualizar uma ferramenta sem suas categorias relacionadas.");
                var tool = await this.toolsServices.GetForUpdate(Id);
                if (tool == null)
                    return NotFound("Ferramenta não encontrada.");
                tool.Update(toolRequest.ImgUrl);
                var requestToolContentIds = toolRequest.toolContents
                    .Where(c => c.Id.HasValue)
                    .Select(c => c.Id!.Value);
                tool.RemoveToolContents(requestToolContentIds);
                foreach (var item in toolRequest.toolContents)
                {
                    if (item.Id is not Guid toolContentId)
                        return BadRequest("Não é possivel atualizar um conteúdo sem seu identificador");
                    var toolContent = await this.toolContentServices.GetById(toolContentId);
                    if (toolContent == null)
                        return NotFound("Conteúdo da ferramenta não encontrada.");
                    toolContent.Update(item.LanguageId, item.Name, item.Description, item.Content, item.Slug);
                    var exists = tool.ToolContents.Any(tc => tc.Id == toolContentId);
                    if (!exists)
                        tool.AddToolContent(toolContent);
                }
                var requestCategoryIds = toolRequest.Categories
                    .Where(c => c.Id.HasValue)
                    .Select(c => c.Id!.Value);
                tool.RemoveCategories(requestCategoryIds);
                foreach (var item in toolRequest.Categories)
                {
                    if (item.Id is not Guid categoryId)
                        return BadRequest("Não é possível atualizar uma categoria sem seu identificador");
                    var exists = tool.Categories.Any(c => c.Id == categoryId);
                    if (exists) continue;
                    var category = await this.categoryServices.GetById(categoryId);
                    if (category == null)
                        return NotFound("Conteúdo da categoria não encontrada.");
                    tool.AddCategory(category);
                }
                await this.toolsServices.Update(tool);
                return Ok(new { message = "Ferramenta atualizada com sucesso." });
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpDelete("{Id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteTool([FromRoute] Guid Id)
        {
            var tool = await this.toolsServices.GetById(Id);
            if (tool == null)
                return NotFound(new { message = "Ferramenta não encontrada." });
            await this.toolsServices.Delete(tool);
            return Ok(new { message = "Ferramenta deletada com sucesso." });
        }
    }
}