using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostService.Application.Interfaces;

namespace PostService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContributorController : ControllerBase
    {
        private readonly IContributorServices contributorServices;
        public ContributorController(
            IContributorServices _contributorServices
        )
        {
            this.contributorServices = _contributorServices;
        }
        [HttpGet("GetByPagination")]
        [Authorize(Roles="Administrador", AuthenticationSchemes ="UserJwt")]
        public async Task<IActionResult> GetByPagination(
            [FromQuery] int page,
            [FromQuery] Guid postId,
            [FromQuery] string? search
        )
        {
            var result = await this.contributorServices.GetByPagination(page, postId, search);
            return Ok(result);
        }
    }
}