using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostService.Application.UseCases.Links.Interfaces;
using PostService.Application.DTOs.Request;
using PostService.Application.Interfaces;
namespace PostService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LinkController : ControllerBase
    {
        private readonly ILinkServices linkServices;
        private readonly ICreateLink createLink;
        private readonly IUpdateLink updateLink;
        private readonly IDeleteLink deleteLink;
        public LinkController(
            ILinkServices _linkServices,
            ICreateLink _createLink,
            IUpdateLink _updateLink,
            IDeleteLink _deleteLink
        )
        {
            this.linkServices = _linkServices;
            this.createLink = _createLink;
            this.updateLink = _updateLink;
            this.deleteLink = _deleteLink;
        }
        [HttpGet("GetByPagination")]
        [Authorize(Roles="Administrador", AuthenticationSchemes ="UserJwt")]
        public async Task<IActionResult> GetByPagination(
            [FromQuery] int page,
            [FromQuery] Guid? toolId,
            [FromQuery] Guid? postId,
            [FromQuery] string? search
        )
        {
            var result = await this.linkServices.GetByPagination(page, toolId, postId, search);
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Roles="Administrador", AuthenticationSchemes ="UserJwt")]
        public async Task<IActionResult> CreateLink([FromBody] LinkRequest request)
        {
            if (ModelState.IsValid)
            {
                await this.createLink.ExecuteAsync(request);
                return Ok(new { message = "Link salvo com sucesso. " });
            }
            var erros = ModelState.Values.Select(e => e.Errors);
            return BadRequest(erros);
        }
        [HttpPut("{Id}")]
        [Authorize(Roles="Administrador", AuthenticationSchemes ="UserJwt")]
        public async Task<IActionResult> UpdateLink([FromRoute] Guid Id, [FromBody] LinkRequest request)
        {
            if (ModelState.IsValid)
            {
                await this.updateLink.ExecuteAsync(Id, request);
                return Ok(new { message = "Link atualizado com sucesso. " });
            }
            var erros = ModelState.Values.Select(e => e.Errors);
            return BadRequest(erros);
        }
        [HttpDelete("{Id}")]
        [Authorize(Roles="Administrador", AuthenticationSchemes ="UserJwt")]
        public async Task<IActionResult> DeleteLink([FromRoute] Guid Id)
        {
            if (ModelState.IsValid)
            {
                await this.deleteLink.ExecuteAsync(Id);
                return Ok(new { message = "Link deletado com sucesso. " });
            }
            var erros = ModelState.Values.Select(e => e.Errors);
            return BadRequest(erros);
        }
    }
}