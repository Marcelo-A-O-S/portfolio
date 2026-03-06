using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostService.Application.DTOs.Request;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;

namespace PostService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToolController : ControllerBase
    {
        private readonly IToolsServices toolsServices;
        private readonly IToolContentServices toolContentServices;
        public ToolController(IToolsServices _toolsServices, IToolContentServices _toolContentServices)
        {
            this.toolsServices = _toolsServices;
            this.toolContentServices = _toolContentServices;
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
                var toolContent = await this.toolContentServices.FindBy(x=> x.Slug == toolRequest.Slug && x.Language == toolRequest.Language);
                if (toolContent != null)
                {
                    return BadRequest(new { message = "Erro ao criar ferramenta!" });
                }
                var tool = new Tool();
                await this.toolsServices.Save(tool);
                toolContent = new ToolContent(tool.Id,toolRequest.Language,toolRequest.Name, toolRequest.Description, toolRequest.Content, toolRequest.Slug);
                await this.toolContentServices.Save(toolContent);
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
                var tool = await this.toolsServices.GetById(Id);
                if(tool == null)
                {
                    return NotFound("Ferramenta não encontrada.");
                }
                var toolContent = await this.toolContentServices.FindBy(tl => tl.Id == toolRequest.Id && tl.ToolId == Id && tl.Language == toolRequest.Language && tl.Slug == toolRequest.Slug);
                if(toolContent == null)
                {
                    return NotFound("Ferramenta não encontrada.");
                }
                toolContent.Update(toolRequest.Language, toolRequest.Name, toolRequest.Description, toolRequest.Content, toolRequest.Slug);
                await this.toolContentServices.Update(toolContent);
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