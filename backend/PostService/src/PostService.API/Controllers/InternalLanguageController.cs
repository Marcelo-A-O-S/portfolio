using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Languages.Interfaces;

namespace PostService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InternalLanguageController : ControllerBase
    {
        private readonly IGetByIdLanguage getByIdLanguage;
        private readonly ILanguageServices languageServices;
        public InternalLanguageController(
            IGetByIdLanguage _getByIdLanguage,
            ILanguageServices _languageServices
        )
        {
            this.getByIdLanguage = _getByIdLanguage;
            this.languageServices = _languageServices;
        }
        [HttpGet("{Id}")]
        [Authorize(AuthenticationSchemes = "InternalJwt", Policy = "UsersRead")]
        public async Task<IActionResult> GetLanguage([FromRoute] Guid Id)
        {
            var result = await this.getByIdLanguage.ExecuteAsync(Id);
            if(result == null)
                return NotFound();
            return Ok();
        }
    }
}