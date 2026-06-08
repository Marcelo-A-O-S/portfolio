using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PostService.Application.DTOs.Request;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Languages.Interfaces;
using PostService.Domain.Entities;

namespace PostService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LanguageController : ControllerBase
    {
        private readonly ILanguageServices languageServices;
        private readonly ICreateLanguage createLanguage;
        private readonly IUpdateLanguage updateLanguage;
        private readonly IDeleteLanguage deleteLanguage;
        public LanguageController(
            ILanguageServices _languageServices,
            ICreateLanguage _createLanguage,
            IUpdateLanguage _updateLanguage,
            IDeleteLanguage _deleteLanguage)
        {
            this.languageServices = _languageServices;
            this.createLanguage = _createLanguage;
            this.updateLanguage = _updateLanguage;
            this.deleteLanguage = _deleteLanguage;
        }
        [HttpGet]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> List([FromQuery] int? page)
        {
            var languages = new List<Language>();
            if (page.HasValue)
            {
                languages = await this.languageServices.List(page ?? 1);
            }
            else
            {
                languages = await this.languageServices.List();
            }
            return Ok(languages);
        }
        [HttpGet("GetByPagination")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        [EnableRateLimiting("pagination")]
        public async Task<IActionResult> GetByPagination(
            [FromQuery] int page,
            [FromQuery] string? search,
            [FromQuery] string? code
        )
        {
            var result = await this.languageServices.GetPagination(page, search, code);
            return Ok(result);
        }
        [HttpGet("{Id:guid}")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            var language = await this.languageServices.GetById(Id);
            return Ok(language);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> CreateLanguage([FromBody] LanguageRequest languageRequest)
        {
            if (ModelState.IsValid)
            {
                await this.createLanguage.ExecuteAsync(languageRequest);
                return Ok(new { message = "Linguagem salva com sucesso. " });
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
        [HttpPut("{Id:guid}")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> UpdateLanguage([FromRoute] Guid Id, [FromBody] LanguageRequest languageRequest)
        {
            if (ModelState.IsValid)
            {
                await this.updateLanguage.ExecuteAsync(Id, languageRequest);
                return Ok(new { message = "Linguagem atualizada com sucesso." });
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
        [HttpDelete("{Id:guid}")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> DeleteByRoute([FromRoute] Guid Id)
        {
            await this.deleteLanguage.ExecuteAsync(Id);
            return Ok(new { message = "Linguagem deletada com sucesso." });
        }
    }
}