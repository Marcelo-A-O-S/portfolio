using System.Security.Claims;
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
        private readonly IAddLinkTool addLinkTool;
        private readonly IUpdateLinkTool updateLinkTool;
        private readonly IDeleteLinkTool deleteLinkTool;
        public ToolController(
            IToolsServices _toolsServices,
            ICreateTool _createTool,
            IUpdateTool _updateTool,
            IDeleteTool _deleteTool,
            IAddLinkTool _addLinkTool,
            IUpdateLinkTool _updateLinkTool,
            IDeleteLinkTool _deleteLinkTool)
        {
            this.toolsServices = _toolsServices;
            this.createTool = _createTool;
            this.updateTool = _updateTool;
            this.deleteTool = _deleteTool;
            this.addLinkTool = _addLinkTool;
            this.updateLinkTool = _updateLinkTool;
            this.deleteLinkTool = _deleteLinkTool;
        }
        [HttpGet("GetTools")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> GetTools()
        {
            var tools = await this.toolsServices.GetTools();
            return Ok(tools);
        }
        [HttpGet]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
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
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            var tool = await this.toolsServices.GetById(Id);
            if (tool == null)
                return NotFound();
            return Ok(tool);
        }
        [HttpGet("GetToolById/{Id}")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> GetToolById([FromRoute] Guid Id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var tool = await this.toolsServices.GetToolById(Guid.Parse(userId), Id);
            if (tool == null)
                return NotFound();
            return Ok(tool);
        }
        [HttpGet("GetByPagination")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> GetByPagination(
            [FromQuery] int page,
            [FromQuery] string? search
        )
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await this.toolsServices.GetByPagination(Guid.Parse(userId), page, search);
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> CreateTool(ToolRequest toolRequest)
        {
            if (ModelState.IsValid)
            {
                var providerId = User.FindFirst("ProviderId")?.Value;
                if (providerId == null)
                    return BadRequest(new { message = "Provider inválido." });
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                if (userId == null || role == null)
                    return Unauthorized(new { message = "Usuário não autorizado." });
                await this.createTool.ExecuteAsync(Guid.Parse(userId), providerId, toolRequest);
                return Ok(new { message = "Ferramenta salva com sucesso." });
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpPut("{Id:guid}")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> UpdateTool([FromRoute] Guid Id, ToolRequest toolRequest)
        {
            if (ModelState.IsValid)
            {
                var providerId = User.FindFirst("ProviderId")?.Value;
                if (providerId == null)
                    return BadRequest(new { message = "Provider inválido." });
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                if (userId == null || role == null)
                    return Unauthorized(new { message = "Usuário não autorizado." });
                await this.updateTool.ExecuteAsync(Guid.Parse(userId), role, Id, toolRequest);
                return Ok(new { message = "Ferramenta atualizada com sucesso." });
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpDelete("{Id:guid}")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> DeleteTool([FromRoute] Guid Id)
        {
            await this.deleteTool.ExecuteAsync(Id);
            return Ok(new { message = "Ferramenta deletada com sucesso." });
        }
        [HttpPost("Link")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> AddLink([FromBody] LinkRequest request)
        {
            if (ModelState.IsValid)
            {
                await this.addLinkTool.ExecuteAsync(request);
                return Ok(new { message = "Link vinculado a ferramenta com sucesso." });
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpPut("Link/{Id}")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> UpdateLink([FromRoute] Guid Id, [FromBody] LinkRequest request)
        {
            if (ModelState.IsValid)
            {
                await this.updateLinkTool.ExecuteAsync(Id,request);
                return Ok(new { message = "Link vinculado a ferramenta atualizado com sucesso." });
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpDelete("Link/{Id}")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> DeleteLink([FromRoute] Guid Id)
        {
            if (ModelState.IsValid)
            {
                await this.deleteLinkTool.ExecuteAsync(Id);
                return Ok(new { message = "Link vinculado a ferramenta removido com sucesso." });
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
    }
}