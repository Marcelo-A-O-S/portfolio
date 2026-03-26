using Microsoft.AspNetCore.Mvc;
using PostService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using PostService.Domain.Entities;
using PostService.Application.DTOs.Request;

namespace PostService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostServices postServices;
        private readonly IPostContentServices postContentServices;
        private readonly ICategoryServices categoryServices;
        private readonly IToolsServices toolsServices;
        public PostController(
            IPostServices _postServices,
            IPostContentServices _postContentServices,
            ICategoryServices _categoryServices,
            IToolsServices _toolsServices
            )
        {
            this.postServices = _postServices;
            this.postContentServices = _postContentServices;
            this.categoryServices = _categoryServices;
            this.toolsServices = _toolsServices;
        }
        [HttpGet]
        [Authorize( Roles = "Administrador")]
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
        [HttpGet("{Id}")]
        [Authorize( Roles = "Administrador")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            var post = await this.postServices.GetById(Id);
            if(post == null)
                return NotFound();
            return Ok(post);
        }
        [HttpGet]
        [Authorize( Roles = "Administrador")]
        public async Task<IActionResult> GetByPagination(
            [FromQuery] int page,
            [FromQuery] string? search
        )
        {
            var result = await this.postServices.GetByPagination(page, search);
            return Ok(result);
        }
        [HttpPost]
        [Authorize( Roles = "Administrador")]
        public async Task<IActionResult> CreatePost(PostRequest postRequest)
        {
            if (ModelState.IsValid)
            {
                if(postRequest.Categories.Count == 0)
                    return BadRequest(new { message = "Não é possível salvar um post sem suas categorias relacionadas."});
                if(postRequest.Tools.Count == 0)
                    return BadRequest(new { message = "Não é possível salvar um post sem suas ferramentas relacionadas."});
                if(postRequest.PostContents.Count == 0)
                    return BadRequest(new { message = "Não é possível salvar um post sem seu conteúdo relacionado."});
                var post = new Post(postRequest.Status);
                foreach(var item in postRequest.Categories)
                {
                    if(item.Id is not Guid categoryId)
                        return BadRequest(new { message = "O identificador relacionado as categorias são obrigatórios."});
                    var category = await this.categoryServices.GetById(categoryId);
                    if(category == null)
                        return NotFound(new { message = "A categoria não foi encontrada"});
                    post.AddCategory(category);
                }
                foreach(var item in postRequest.Tools)
                {
                    if(item.Id is not Guid toolId)
                        return BadRequest(new { message = "O identificador relacionado as ferramentas são obrigatórios." });
                    var tool = await this.toolsServices.GetById(toolId);
                    if(tool == null)
                        return NotFound(new { message = "A ferramenta não foi encontrada"});
                    post.AddTool(tool);
                }
                foreach(var item in postRequest.PostContents)
                {
                    var postContent = await this.postContentServices.FindBy(pc => pc.Slug == item.Slug && pc.LanguageId == item.LanguageId);
                    if(postContent != null)
                        return BadRequest("Erro ao validar dados.");
                    postContent = new PostContent(post.Id, item.LanguageId, item.Title, item.Description, item.Content, item.Slug);
                    post.AddPostContent(postContent);
                }
                await this.postServices.Save(post);
                return Ok(new { message= "Projeto salvo com sucesso!"});
            }
            var errors = ModelState.Values.Select(e=> e.Errors);
            return BadRequest(errors);
        }
        [HttpPut("{Id}")]
        [Authorize( Roles = "Administrador")]
        public async Task<IActionResult> UpdatePost([FromRoute] Guid Id, PostRequest postRequest)
        {
            if (ModelState.IsValid)
            {
                if(postRequest.Categories.Count == 0)
                    return BadRequest(new { message = "Não é possível salvar um post sem suas categorias relacionadas."});
                if(postRequest.Tools.Count == 0)
                    return BadRequest(new { message = "Não é possível salvar um post sem suas ferramentas relacionadas."});
                if(postRequest.PostContents.Count == 0)
                    return BadRequest(new { message = "Não é possível salvar um post sem seu conteúdo relacionado."});
                var post = await this.postServices.GetForUpdate(Id);
                if(post == null)
                    return NotFound(new { message = "Postagem não encontrada."});
                post.Update(postRequest.Status);
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
    }
}