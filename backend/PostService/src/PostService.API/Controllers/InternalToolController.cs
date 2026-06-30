using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostService.Application.UseCases.InternalTool.Interfaces;

namespace PostService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InternalToolController : ControllerBase
    {
        private readonly IExistsByIdTool existsByIdTool;
        public InternalToolController(
            IExistsByIdTool _existsByIdTool
        )
        {
            this.existsByIdTool = _existsByIdTool;
        }
        [HttpGet("{Id}/exists")]
        [Authorize(AuthenticationSchemes ="InternalJwt", Policy="UsersRead")]
        public async Task<IActionResult> ToolExists([FromRoute] Guid Id)
        {
            var exists = await this.existsByIdTool.ExecuteAsync(Id);
            if(!exists)
                return NotFound();
            return Ok(exists);
        }
    }
}