using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PostService.Application.UseCases.InternalProject.Interfaces;
namespace PostService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InternalPostController : ControllerBase
    {
        private readonly IExistsByIdProject existsByIdProject;
        public InternalPostController(
            IExistsByIdProject _existsByIdProject
        )
        {
            this.existsByIdProject = _existsByIdProject;
        }
        [Authorize(AuthenticationSchemes ="InternalJwt", Policy="UsersRead")]
        [HttpGet("internal/post/{Id}/exists")]
        public async Task<IActionResult> PostExists([FromRoute] Guid Id)
        {
            var exists = await existsByIdProject.ExecuteAsync(Id);
            if(!exists)
                return NotFound();
            return Ok(exists);
        }
    }
}