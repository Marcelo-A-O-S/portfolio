using Microsoft.AspNetCore.Mvc;
using PostService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace PostService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostServices postServices;
        public PostController(IPostServices _postServices)
        {
            this.postServices = _postServices;
        }
        [HttpGet]
        [Authorize( Roles = "Administrador")]
        public async Task<IActionResult> List()
        {
            
            return Ok();
        }
    }
}