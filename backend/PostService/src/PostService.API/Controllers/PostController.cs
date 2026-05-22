using Microsoft.AspNetCore.Mvc;
using PostService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using PostService.Domain.Entities;
using PostService.Application.DTOs.Request;
using PostService.Application.UseCases.Projects.Interfaces;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using PostService.Application.UseCases.Likes.Interfaces;

namespace PostService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostServices postServices;
        private readonly ICreateProject createProject;
        private readonly IUpdateProject updateProject;
        private readonly IDeleteProject deleteProject;
        private readonly IAddLike addLike;
        public PostController(
            IPostServices _postServices,
            ICreateProject _createProject,
            IUpdateProject _updateProject,
            IDeleteProject _deleteProject,
            IAddLike _addLike
            )
        {
            this.postServices = _postServices;
            this.createProject = _createProject;
            this.updateProject = _updateProject;
            this.deleteProject = _deleteProject;
            this.addLike = _addLike;
        }
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> List([FromQuery] int? page)
        {
            var posts = new List<Post>();
            if (page.HasValue)
            {
                posts = await this.postServices.List(page ?? 1);
            }
            else
            {
                posts = await this.postServices.List();
            }
            return Ok(posts);
        }
        [HttpGet("{Id:guid}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            var post = await this.postServices.GetById(Id);
            if (post == null)
                return NotFound();
            return Ok(post);
        }
        [HttpGet("GetPostById/{Id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetPostById([FromRoute] Guid Id)
        {
            var post = await this.postServices.GetPostById(Id);
            if (post == null)
                return NotFound();
            return Ok(post);
        }
        [HttpGet("GetByPagination")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetByPagination(
            [FromQuery] int page,
            [FromQuery] string? search
        )
        {
            var result = await this.postServices.GetByPagination(page, search);
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CreatePost([FromForm] PostRequest postRequest)
        {
            if (ModelState.IsValid)
            {
                await this.createProject.ExecuteAsync(postRequest);
                return Ok(new { message = "Projeto salvo com sucesso!" });
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
        [HttpPut("{Id:guid}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> UpdatePost([FromRoute] Guid Id, PostRequest postRequest)
        {
            if (ModelState.IsValid)
            {
                await this.updateProject.ExecuteAsync(Id, postRequest);
                return Ok(new { message = "Postagem atualizada com sucesso!" });
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
        [HttpDelete("{Id:guid}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeletePost(Guid Id)
        {
            await this.deleteProject.ExecuteAsync(Id);
            return Ok(new { message = "Postagem deletada com sucesso!" });
        }
        [HttpPost("{Id:guid}/Like")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AddLike([FromBody] LikeRequest likeRequest)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if(userId == null)
                return Unauthorized();
            await this.addLike.ExecuteAsync(Guid.Parse(userId), likeRequest);
            return Ok();
        }
    }
}