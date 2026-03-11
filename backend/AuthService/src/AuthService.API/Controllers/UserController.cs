using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthService.Application.DTOs.Request;
using Microsoft.AspNetCore.RateLimiting;

namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices userServices;
        public UserController(IUserServices _userServices)
        {
            this.userServices = _userServices;
        }
        [HttpGet]
        [Authorize(Roles = "Administrador,Client")]
        [EnableRateLimiting("read")]
        public async Task<IActionResult> List([FromQuery] int? page)
        {
            var users = new List<User>();
            if (page.HasValue)
            {
                users = await this.userServices.List(page ?? 1);
            }
            else
            {
                users = await this.userServices.List();
            }
            return Ok(users);
        }
        [HttpGet("{Id}")]
        [Authorize(Roles = "Administrador")]
        [EnableRateLimiting("read")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            var user = await this.userServices.GetById(Id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }
        [HttpGet("GetByPagination")]
        [Authorize(Roles = "Administrador,Client")]
        [EnableRateLimiting("pagination")]
        public async Task<IActionResult> GetByPagination(
            [FromQuery] int page,
            [FromQuery] string? search,
            [FromQuery] string? role,
            [FromQuery] string? status)
        {
            var users = await this.userServices.GetByPagination(page, search, role, status);
            return Ok(users);
        }
        [HttpPatch("{Id}/ModifyRole")]
        [Authorize(Roles = "Administrador,Client")]
        [EnableRateLimiting("patch")]
        public async Task<IActionResult> ModifyRole([FromRoute] Guid Id, [FromBody] UpdateRoleRequest updateRoleRequest)
        {
            if (ModelState.IsValid)
            {
                var user = await this.userServices.GetById(Id);
                if (user == null)
                    return NotFound();
                user.UpdateRole(updateRoleRequest.Role);
                await this.userServices.Update(user);
                return Ok(new { message = "Função de usuário atualizada com sucesso!" });
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
        [HttpDelete("{Id}")]
        [Authorize(Roles = "Administrador")]
        [EnableRateLimiting("sensitive")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid Id)
        {
            var user = await this.userServices.GetById(Id);
            if (user == null)
                return NotFound();
            await this.userServices.Delete(user);
            return Ok(new { message = "Usuário deletado com sucesso!" });
        }
        [HttpPatch("{Id}/BanUser")]
        [Authorize(Roles = "Administrador")]
        [EnableRateLimiting("sensitive")]
        public async Task<IActionResult> BanUser([FromRoute] Guid Id)
        {
            var user = await this.userServices.GetById(Id);
            if (user == null)
                return NotFound();
            user.Ban();
            await this.userServices.Update(user);
            return Ok(new { message = "Usuário banido com sucesso!" });
        }
    }
}