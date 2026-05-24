using AuthService.Application.UseCases.Users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InternalUserController : ControllerBase
    {
        private readonly IExistsByIdUser existsByIdUser;
        public InternalUserController(
            IExistsByIdUser _existsByIdUser
        )
        {
            this.existsByIdUser = _existsByIdUser;
        }
        [Authorize(Policy="InternalService")]
        [HttpGet("internal/users/{Id}/exists")]
        public async Task<IActionResult> UserExists([FromRoute] Guid Id)
        {
            var exists = await existsByIdUser.ExecuteAsync(Id);
            return Ok(exists);
        }
    }
}