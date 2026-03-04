using Microsoft.AspNetCore.Mvc;

namespace PostService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(){
            return Ok();
        }
    }
}