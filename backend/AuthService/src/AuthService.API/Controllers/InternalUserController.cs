using AuthService.Application.UseCases.InternalUser.Interfaces;
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
        [HttpGet("{Id}/exists")]
        [Authorize(AuthenticationSchemes="InternalJwt", Policy="UsersRead")]
        public async Task<IActionResult> UserExists([FromRoute] Guid Id)
        {
            var exists = await existsByIdUser.ExecuteAsync(Id);
            if(!exists)
                return NotFound();
            return Ok(exists);
        }
    }
}