using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostService.Application.DTOs.Request;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Tools.Interfaces;
using PostService.Domain.Entities;
namespace PostService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToolController : ControllerBase
    {
        private readonly IToolsServices toolsServices;
        private readonly ICreateTool createTool;
        private readonly IUpdateTool updateTool;
        private readonly IDeleteTool deleteTool;
        public ToolController(
            IToolsServices _toolsServices,
            ICreateTool _createTool,
            IUpdateTool _updateTool,
            IDeleteTool _deleteTool)
        {
            this.toolsServices = _toolsServices;
            this.createTool = _createTool;
            this.updateTool = _updateTool;
            this.deleteTool = _deleteTool;
        }
        [HttpGet("GetTools")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetTools()
        {
            var tools = await this.toolsServices.GetTools();
            return Ok(tools);
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
        [HttpGet("{Id:guid}")]
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
        public async Task<IActionResult> CreateTool([FromForm] ToolRequest toolRequest)
        {
            if (ModelState.IsValid)
            {
                await this.createTool.ExecuteAsync(toolRequest);
                return Ok(new { message = "Ferramenta salva com sucesso." });
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpPut("{Id:guid}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> UpdateTool([FromRoute] Guid Id, ToolRequest toolRequest)
        {
            if (ModelState.IsValid)
            {
                await this.updateTool.ExecuteAsync(Id, toolRequest);
                return Ok(new { message = "Ferramenta atualizada com sucesso." });
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpDelete("{Id:guid}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteTool([FromRoute] Guid Id)
        {
            await this.deleteTool.ExecuteAsync(Id);
            return Ok(new { message = "Ferramenta deletada com sucesso." });
        }
    }
}