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
        [Authorize( Roles = "Administrador")]
        public async Task<IActionResult> List([FromQuery]int? page)
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
        [Authorize( Roles = "Administrador")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            var tool = await this.toolsServices.GetById(Id);
            if(tool == null)
                return NotFound();
            return Ok(tool);
        }
        [HttpPost]
        [Authorize( Roles = "Administrador")]
        public async Task<IActionResult> CreateTool(ToolRequest toolRequest)
        {
            if (ModelState.IsValid)
            {
                var tool = new Tool();
                if(toolRequest.toolContents.Count == 0)
                    return BadRequest("Não é possível salvar uma ferramenta sem seu conteudo relacionado.");
                if(toolRequest.Categories.Count == 0)
                    return BadRequest("Não é possível salvar uma ferramenta sem suas categorias relacionadas.");
                foreach (var item in toolRequest.toolContents)
                {
                    var toolContent = await this.toolContentServices.FindBy(tc => tc.Slug == item.Slug && tc.Language == item.Language);
                    if(toolContent != null)
                        return BadRequest(new { message = "Erro ao validar dados!"});
                    toolContent = new ToolContent(tool.Id, item.Language, item.Name, item.Description, item.Content,item.Slug);
                    tool.AddToolContent(toolContent);
                }
                foreach(var item in toolRequest.Categories)
                {
                    if(item.Id is not Guid categoryId)
                        return BadRequest(new { message = "O identificador relacionado as categorias são obrigatórios"});
                    var category = await this.categoryServices.GetById(categoryId);
                    if(category == null)
                        return NotFound(new { message = "Categoria não encontrada."});
                    tool.AddCategory(category);
                }
                await this.toolsServices.Save(tool);
                return Ok(new { message = "Ferramenta salva com sucesso." });
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpPut("{Id}")]
        [Authorize( Roles = "Administrador")]
        public async Task<IActionResult> UpdateTool([FromRoute] Guid Id, ToolRequest toolRequest)
        {
            if (ModelState.IsValid)
            {
                
                return Ok(new { message = "Ferramenta atualizada com sucesso." });
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpDelete("{Id}")]
        [Authorize( Roles = "Administrador")]
        public async Task<IActionResult> DeleteTool([FromRoute] Guid Id)
        {
            var tool = await this.toolsServices.GetById(Id);
            if(tool == null)
                return NotFound(new { message = "Ferramenta não encontrada." });
            await this.toolsServices.Delete(tool);
            return Ok(new { message = "Ferramenta deletada com sucesso." });
        }
    }
}