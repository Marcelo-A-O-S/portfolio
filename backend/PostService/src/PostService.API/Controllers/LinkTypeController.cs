using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostService.Application.DTOs.Request;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.LinkTypes.Interfaces;
using PostService.Domain.Entities;
namespace PostService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LinkTypeController : ControllerBase
    {
        private readonly ILinkTypeServices linkTypeServices;
        private readonly ICreateLinkType createLinkType;
        private readonly IUpdateLinkType updateLinkType;
        private readonly IDeleteLinkType deleteLinkType;
        public LinkTypeController(
            ILinkTypeServices _linkTypeServices,
            ICreateLinkType _createLinkType,
            IUpdateLinkType _updateLinkType,
            IDeleteLinkType _deleteLinkType
        )
        {
            this.linkTypeServices = _linkTypeServices;
            this.createLinkType = _createLinkType;
            this.updateLinkType = _updateLinkType;
            this.deleteLinkType = _deleteLinkType;
        }
        [HttpGet]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> GetAll([FromQuery] int? page)
        {
            var linkTypes = new List<LinkType>();
            if (page.HasValue)
            {
                linkTypes = await this.linkTypeServices.List(page ?? 1);
            }
            else
            {
                linkTypes = await this.linkTypeServices.List();
            }
            return Ok(linkTypes);
        }
        [HttpGet("GetByPagination")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> GetByPagination(
            [FromQuery] int page,
            [FromQuery] string? search
        )
        {
            var result = await this.linkTypeServices.GetByPagination(page, search);
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> CreateLinkType([FromBody] LinkTypeRequest request)
        {
            if (ModelState.IsValid)
            {
                await this.createLinkType.ExecuteAsync(request);
                return Ok(new { message = "Tipo de Link salvo com sucesso." });
            }
            var erros = ModelState.Values.Select(e => e.Errors);
            return BadRequest(erros);
        }
        [HttpPut("{Id}")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> UpdateLinkType([FromRoute] Guid Id, [FromBody] LinkTypeRequest request)
        {
            if (ModelState.IsValid)
            {
                await this.updateLinkType.ExecuteAsync(Id, request);
                return Ok(new { message = "Tipo de Link atualizado com sucesso." });
            }
            var erros = ModelState.Values.Select(e => e.Errors);
            return BadRequest(erros);
        }
        [HttpDelete("{Id}")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> DeleteLinkType([FromRoute] Guid Id)
        {
            if (ModelState.IsValid)
            {
                await this.deleteLinkType.ExecuteAsync(Id);
                return Ok(new { message = "Tipo de Link deletado com sucesso." });
            }
            var erros = ModelState.Values.Select(e => e.Errors);
            return BadRequest(erros);
        }
    }
}