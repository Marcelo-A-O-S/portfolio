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
        public ToolController(IToolsServices _toolsServices)
        {
            this.toolsServices = _toolsServices;
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
                var tool = await this.toolsServices.FindBy(x=> x.Slug == toolRequest.Slug);
                if (tool != null)
                {
                    return BadRequest(new { message = "Identificador de URL inválido, corrija para prosseguir!" });
                }
                tool = new Tool(toolRequest.Name, toolRequest.Description, toolRequest.Content, toolRequest.Slug);
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
                var tool = await this.toolsServices.GetById(Id);
                if(tool == null)
                {
                    return NotFound("Ferramenta não encontrada.");
                }
                tool.Update(toolRequest.Name, toolRequest.Description, toolRequest.Content, toolRequest.Slug);
                await this.toolsServices.Update(tool);
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
                return NotFound(new { message = "Pedido não encontrado" });
            await this.toolsServices.Delete(tool);
            return Ok(new { message = "Ferramenta deletada com sucesso." });
        }
    }
}