using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostService.Application.DTOs.Request;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;

namespace PostService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LanguageController : ControllerBase
    {
        private readonly ILanguageServices languageServices;
        public LanguageController(ILanguageServices _languageServices)
        {
            this.languageServices = _languageServices;
        }
        [HttpGet]
        [Authorize( Roles = "Administrador")]
        public async Task<IActionResult> List([FromQuery]int? page)
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
        [Authorize( Roles = "Administrador")]
        public async Task<IActionResult> GetByPagination(
            [FromQuery] int page,
            [FromQuery] string? search,
            [FromQuery] string? code
        )
        {
            var result = await this.languageServices.GetPagination(page, search, code);
            return Ok(result);
        }
        [HttpGet("{Id}")]
        [Authorize( Roles = "Administrador")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            var language = await this.languageServices.GetById(Id);
            return Ok(language);
        }
        [HttpPost]
        [Authorize( Roles = "Administrador")]
        public async Task<IActionResult> CreateLanguage([FromBody] LanguageRequest languageRequest)
        {
            if (ModelState.IsValid)
            {
                var language = await this.languageServices.FindBy(lg => lg.Code == languageRequest.Code && lg.Name == languageRequest.Name);
                if(language != null)
                    return BadRequest(new { message = "Erro ao validar os dados." });
                language = new Language(languageRequest.Code, languageRequest.Name);
                await this.languageServices.Save(language);
                return Ok(new { message = "Linguagem salva com sucesso. "});
            }
            var errors = ModelState.Values.Select(e=> e.Errors);
            return BadRequest(errors);
        }
        [HttpPut("{Id}")]
        [Authorize( Roles = "Administrador")]
        public async Task<IActionResult> UpdateLanguage([FromRoute] Guid Id, [FromBody] LanguageRequest languageRequest)
        {
            if (ModelState.IsValid)
            {
                if(languageRequest.Id is not Guid languageId)
                    return BadRequest(new { message= "O identificador é obrigatório"});
                var language = await this.languageServices.GetById(languageId);
                if(language == null)
                    return NotFound(new { message = "Linguagem não encontrada."});
                language.Update(language.Code, language.Name);
                await this.languageServices.Update(language);
                return Ok(new { message = "Linguagem atualizada com sucesso."});
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
        [HttpDelete("{Id}")]
        [Authorize( Roles = "Administrador")]
        public async Task<IActionResult> DeleteByRoute([FromRoute] Guid Id)
        {
            var language = await this.languageServices.GetById(Id);
            if(language == null)
                return NotFound(new { message = "Linguagem não encontrada."});
            await this.languageServices.Delete(language);
            return Ok(new { message ="Linguagem deletada com sucesso."});
        }
    }
}