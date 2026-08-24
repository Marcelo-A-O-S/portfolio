using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostService.Application.DTOs.Request;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Contributors.Interfaces;
using PostService.Application.UseCases.Projects.Interfaces;
using PostService.Domain.Entities;
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
        private readonly IAddLinkProject addLinkProject;
        private readonly IUpdateLinkProject updateLinkProject;
        private readonly IDeleteLinkProject deleteLinkProject;
        private readonly IAddContributor addContributor;
        private readonly IUpdateContributor updateContributor;
        private readonly IDeleteContributor deleteContributor;
        public PostController(
            IPostServices _postServices,
            ICreateProject _createProject,
            IUpdateProject _updateProject,
            IDeleteProject _deleteProject,
            IAddLinkProject _addLinkProject,
            IUpdateLinkProject _updateLinkProject,
            IDeleteLinkProject _deleteLinkProject,
            IAddContributor _addContributor,
            IUpdateContributor _updateContributor,
            IDeleteContributor _deleteContributor
            )
        {
            this.postServices = _postServices;
            this.createProject = _createProject;
            this.updateProject = _updateProject;
            this.deleteProject = _deleteProject;
            this.addLinkProject = _addLinkProject;
            this.updateLinkProject = _updateLinkProject;
            this.deleteLinkProject = _deleteLinkProject;
            this.addContributor = _addContributor;
            this.updateContributor = _updateContributor;
            this.deleteContributor = _deleteContributor;
        }
        [HttpGet]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
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
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            var post = await this.postServices.GetById(Id);
            if (post == null)
                return NotFound();
            return Ok(post);
        }
        [HttpGet("GetPostById/{Id}")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> GetPostById([FromRoute] Guid Id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var post = await this.postServices.GetPostById(Guid.Parse(userId), Id);
            if (post == null)
                return NotFound();
            return Ok(post);
        }
        [HttpGet("GetByPagination")]
        [Authorize(Roles = "Administrador,Client", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> GetByPagination(
            [FromQuery] int page,
            [FromQuery] string? search
        )
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();
            var result = await this.postServices.GetByPagination(Guid.Parse(userId), page, search);
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> CreatePost(PostRequest postRequest)
        {
            if (ModelState.IsValid)
            {
                var providerId = User.FindFirst("ProviderId")?.Value;
                if (providerId == null)
                    return BadRequest(new { message = "Provider inválido." });
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                if (userId == null || role == null)
                    return Unauthorized(new { message = "Usuário não autorizado." });
                await this.createProject.ExecuteAsync(Guid.Parse(userId), providerId, postRequest);
                return Ok(new { message = "Projeto salvo com sucesso!" });
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
        [HttpPut("{Id:guid}")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> UpdatePost([FromRoute] Guid Id, PostRequest postRequest)
        {
            if (ModelState.IsValid)
            {
                var providerId = User.FindFirst("ProviderId")?.Value;
                if (providerId == null)
                    return BadRequest(new { message = "Provider inválido." });
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                if (userId == null || role == null)
                    return Unauthorized(new { message = "Usuário não autorizado." });
                await this.updateProject.ExecuteAsync(Guid.Parse(userId), role, Id, postRequest);
                return Ok(new { message = "Postagem atualizada com sucesso!" });
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
        [HttpDelete("{Id:guid}")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> DeletePost(Guid Id)
        {
            await this.deleteProject.ExecuteAsync(Id);
            return Ok(new { message = "Postagem deletada com sucesso!" });
        }
        [HttpPost("Link")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> AddLink([FromBody] LinkRequest request)
        {
            if (ModelState.IsValid)
            {
                await this.addLinkProject.ExecuteAsync(request);
                return Ok(new { message = "Link vinculado ao projeto com sucesso." });
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpPut("Link/{Id}")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> UpdateLink([FromRoute] Guid Id, [FromBody] LinkRequest request)
        {
            if (ModelState.IsValid)
            {
                await this.updateLinkProject.ExecuteAsync(Id, request);
                return Ok(new { message = "Link vinculado ao projeto atualizado com sucesso." });
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpDelete("Link/{Id}")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> DeleteLink([FromRoute] Guid Id)
        {
            if (ModelState.IsValid)
            {
                await this.deleteLinkProject.ExecuteAsync(Id);
                return Ok(new { message = "Link vinculado ao projeto removido com sucesso." });
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpPost("Contributor")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> AddContributor([FromBody] ContributorRequest request)
        {
            if (ModelState.IsValid)
            {
                await this.addContributor.ExecuteAsync(request);
                return Ok(new { message = "Contribuidor vinculado ao projeto com sucesso." });
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpPut("Contributor/{Id}")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> UpdateContributor([FromRoute] Guid Id, [FromBody] ContributorRequest request)
        {
            if (ModelState.IsValid)
            {
                await this.updateContributor.ExecuteAsync(Id, request);
                return Ok(new { message = "Contribuidor vinculado ao projeto atualizado com sucesso." });
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpDelete("Contributor/{Id}")]
        [Authorize(Roles = "Administrador", AuthenticationSchemes = "UserJwt")]
        public async Task<IActionResult> DeleteContributor([FromRoute] Guid Id)
        {
            if (ModelState.IsValid)
            {
                await this.deleteContributor.ExecuteAsync(Id);
                return Ok(new { message = "Contribuidor vinculado ao projeto removido com sucesso." });
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
    }
}