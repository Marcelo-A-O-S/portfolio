using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostService.Application.DTOs.Request;
using PostService.Application.Interfaces;
using PostService.Application.Validations;
using PostService.Domain.Entities;

namespace PostService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToolController : ControllerBase
    {
        private readonly IFileServices fileServices;
        private readonly IToolsServices toolsServices;
        private readonly IToolContentServices toolContentServices;
        private readonly ICategoryServices categoryServices;
        private readonly IMediaFileServices mediaFileServices;
        public ToolController(
            IToolsServices _toolsServices,
            IToolContentServices _toolContentServices,
            ICategoryServices _categoryServices,
            IFileServices _fileServices,
            IMediaFileServices _mediaFileServices)
        {
            this.toolsServices = _toolsServices;
            this.toolContentServices = _toolContentServices;
            this.categoryServices = _categoryServices;
            this.fileServices = _fileServices;
            this.mediaFileServices = _mediaFileServices;
        }
        [HttpGet("GetTools")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetTools()
        {
            var tools = await this.toolsServices.GetTools();
            return Ok(tools);
        }
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> List([FromQuery] int? page)
        {
            var tools = new List<Tool>();
            if (page.HasValue)
            {
                tools = await this.toolsServices.List(page ?? 1);
            }
            else
            {
                tools = await this.toolsServices.List();
            }
            return Ok(tools);
        }
        [HttpGet("{Id:guid}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            var tool = await this.toolsServices.GetById(Id);
            if (tool == null)
                return NotFound();
            return Ok(tool);
        }
        [HttpGet("GetToolById/{Id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetToolById([FromRoute] Guid Id)
        {
            var tool = await this.toolsServices.GetToolById(Id);
            if (tool == null)
                return NotFound();
            return Ok(tool);
        }
        [HttpGet("GetByPagination")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetByPagination(
            [FromQuery] int page,
            [FromQuery] string? search
        )
        {
            var result = await this.toolsServices.GetByPagination(page, search);
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CreateTool([FromForm] ToolRequest toolRequest)
        {
            if (ModelState.IsValid)
            {
                if (toolRequest.ImgUrl == null)
                    return BadRequest(new { message = "Imagem é obrigatória." });
                if (!toolRequest.ImgUrl.ContentType.StartsWith("image/"))
                    return BadRequest(new { message = "Arquivo deve ser uma imagem." });
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var toolContentRequest = JsonSerializer.Deserialize<List<ToolContentRequest>>(toolRequest.ToolContents, options);
                if (toolContentRequest is null || toolContentRequest.Count == 0)
                    return BadRequest(new { message = "Não é possivel salvar uma ferramenta sem seu conteudo relacionado." });
                var categoriesRequest = JsonSerializer.Deserialize<List<CategoryRequest>>(toolRequest.Categories, options);
                if (categoriesRequest is null || categoriesRequest.Count == 0)
                    return BadRequest(new { message = "Não é possivel salvar uma ferramenta sem suas categorias relacionadas." });
                var mediasToCommit = new List<MediaFile>();
                var tool = new Tool(toolRequest.Status);
                foreach (var item in toolContentRequest)
                {
                    var validationError = ValidationHelper.Validate(item);
                    if (validationError.Count > 0)
                        return BadRequest(validationError);
                    var toolContent = await this.toolContentServices.FindBy(tc => tc.Slug == item.Slug && tc.LanguageId == item.LanguageId);
                    if (toolContent != null)
                        return BadRequest(new { message = "Erro ao validar dados!" });
                    toolContent = new ToolContent(tool.Id, item.LanguageId, item.Name, item.Description, item.Content, item.Slug);
                    var toRemoveImages = item.ImagesUrls.Where(image => !item.Content.Contains(image)).ToList();
                    foreach (var removeImageUrl in toRemoveImages)
                    {
                        var mediaFileContent = await this.mediaFileServices.GetByPath(removeImageUrl);
                        if (mediaFileContent != null)
                            await this.mediaFileServices.DeleteImageAsync(mediaFileContent);
                        item.ImagesUrls.Remove(removeImageUrl);
                    }
                    foreach (var addImageUrl in item.ImagesUrls)
                    {
                        var mediaFileContent = await this.mediaFileServices.GetByPath(addImageUrl);
                        if (mediaFileContent != null)
                        {
                            mediasToCommit.Add(mediaFileContent);
                        }
                    }
                    toolContent.SetImagesUrls(item.ImagesUrls);
                    tool.AddToolContent(toolContent);
                }
                foreach (var item in categoriesRequest)
                {
                    var validationError = ValidationHelper.Validate(item);
                    if (validationError.Count > 0)
                        return BadRequest(validationError);
                    if (item.Id is not Guid categoryId)
                        return BadRequest(new { message = "O identificador relacionado as categorias são obrigatórios" });
                    var category = await this.categoryServices.GetById(categoryId);
                    if (category == null)
                        return NotFound(new { message = "Categoria não encontrada." });
                    tool.AddCategory(category);
                }
                var mediaFileTool = await this.mediaFileServices.SaveImageAsync(toolRequest.ImgUrl, "media/tools");
                if (mediaFileTool is null)
                    return BadRequest(new { message = "Erro ao salvar imagem." });
                tool.AddImgUrl(mediaFileTool.Path);
                mediasToCommit.Add(mediaFileTool);
                await this.toolsServices.Save(tool);
                foreach (var media in mediasToCommit)
                {
                    media.Commit();
                    await this.mediaFileServices.Update(media);
                }
                return Ok(new { message = "Ferramenta salva com sucesso." });
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpPut("{Id:guid}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> UpdateTool([FromRoute] Guid Id, ToolRequest toolRequest)
        {
            if (ModelState.IsValid)
            {
                if (toolRequest.ImgUrl == null)
                    return BadRequest(new { message = "Imagem é obrigatória." });
                if (!toolRequest.ImgUrl.ContentType.StartsWith("image/"))
                    return BadRequest(new { message = "Arquivo deve ser uma imagem." });
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var toolContentRequest = JsonSerializer.Deserialize<List<ToolContentRequest>>(toolRequest.ToolContents, options);
                if (toolContentRequest is null || toolContentRequest.Count == 0)
                    return BadRequest(new { message = "Não é possivel atualizar uma ferramenta sem seu conteudo relacionado." });
                var categoriesRequest = JsonSerializer.Deserialize<List<CategoryRequest>>(toolRequest.Categories, options);
                if (categoriesRequest is null || categoriesRequest.Count == 0)
                    return BadRequest(new { message = "Não é possivel atualizar uma ferramenta sem suas categorias relacionadas." });
                var mediasToCommit = new List<MediaFile>();
                var tool = await this.toolsServices.GetForUpdate(Id);
                if (tool == null)
                    return NotFound(new { message = "Ferramenta não encontrada." });
                var requestToolContentIds = toolContentRequest
                    .Where(c => c.Id.HasValue)
                    .Select(c => c.Id!.Value);
                tool.ValidateToolContents(requestToolContentIds);
                foreach (var item in toolContentRequest)
                {
                    if (item.Id.HasValue)
                    {
                        var toolContent = tool.ToolContents.FirstOrDefault(tc => tc.Id == item.Id.Value);
                        if (toolContent == null)
                            return NotFound(new { message = "Conteúdo da ferramenta não encontrada." });
                        toolContent.Update(item.LanguageId, item.Name, item.Description, item.Content, item.Slug);
                        var toRemoveImages = item.ImagesUrls.Where(image => !item.Content.Contains(image)).ToList();
                        foreach (var removeImageUrl in toRemoveImages)
                        {
                            var removeMediaFileContent = await this.mediaFileServices.GetByPath(removeImageUrl);
                            if (removeMediaFileContent != null)
                                await this.mediaFileServices.DeleteImageAsync(removeMediaFileContent);
                            item.ImagesUrls.Remove(removeImageUrl);
                        }
                        foreach (var addImageUrl in item.ImagesUrls)
                        {
                            var addMediaFileContent = await this.mediaFileServices.GetByPath(addImageUrl);
                            if (addMediaFileContent != null)
                            {
                                mediasToCommit.Add(addMediaFileContent);
                            }
                        }
                        toolContent.SetImagesUrls(item.ImagesUrls);
                    }
                    else
                    {
                        var toolContent = new ToolContent(tool.Id, item.LanguageId, item.Name, item.Description, item.Content, item.Slug);
                        var toRemoveImages = item.ImagesUrls.Where(image => !item.Content.Contains(image)).ToList();
                        foreach (var removeImageUrl in toRemoveImages)
                        {
                            var removeMediaFileContent = await this.mediaFileServices.GetByPath(removeImageUrl);
                            if (removeMediaFileContent != null)
                                await this.mediaFileServices.DeleteImageAsync(removeMediaFileContent);
                            item.ImagesUrls.Remove(removeImageUrl);
                        }
                        foreach (var addImageUrl in item.ImagesUrls)
                        {
                            var addMediaFileContent = await this.mediaFileServices.GetByPath(addImageUrl);
                            if (addMediaFileContent != null)
                            {
                                mediasToCommit.Add(addMediaFileContent);
                            }
                        }
                        toolContent.SetImagesUrls(item.ImagesUrls);
                        tool.AddToolContent(toolContent);
                    }
                }
                var requestCategoryIds = categoriesRequest
                    .Where(c => c.Id.HasValue)
                    .Select(c => c.Id!.Value);
                tool.ValidateCategories(requestCategoryIds);
                foreach (var item in categoriesRequest)
                {
                    if (item.Id is not Guid categoryId)
                        return BadRequest(new { message= "Não é possível atualizar uma categoria sem seu identificador"});
                    var exists = tool.Categories.Any(c => c.Id == categoryId);
                    if (exists) continue;
                    var category = await this.categoryServices.GetById(categoryId);
                    if (category == null)
                        return NotFound(new { message= "Conteúdo da categoria não encontrada."});
                    tool.AddCategory(category);
                }
                var searchMediaFileContent = await this.mediaFileServices.GetByPath(toolRequest.ImgUrl.FileName);
                if (searchMediaFileContent is not null)
                {
                    mediasToCommit.Add(searchMediaFileContent);
                }
                else
                {

                }
                await this.toolsServices.Update(tool);
                foreach (var media in mediasToCommit)
                {
                    media.Commit();
                    await this.mediaFileServices.Update(media);
                }
                return Ok(new { message = "Ferramenta atualizada com sucesso." });
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpDelete("{Id:guid}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteTool([FromRoute] Guid Id)
        {
            var tool = await this.toolsServices.GetById(Id);
            var mediaToDelete = new List<MediaFile>();
            if (tool == null)
                return NotFound(new { message = "Ferramenta não encontrada." });
            var mediaImgUrl = await this.mediaFileServices.GetByPath(tool.ImgUrl);
            if(mediaImgUrl != null)
                mediaToDelete.Add(mediaImgUrl);
            foreach(var toolContent in tool.ToolContents)
            {
                foreach(var imagePath in toolContent.ImagesUrls)
                {
                    var mediaImageContent = await this.mediaFileServices.GetByPath(imagePath);
                    if(mediaImageContent != null)
                        mediaToDelete.Add(mediaImageContent);
                }
            }
            await this.toolsServices.Delete(tool);
            foreach(var media in mediaToDelete)
            {
                await this.mediaFileServices.DeleteImageAsync(media);
            }
            return Ok(new { message = "Ferramenta deletada com sucesso." });
        }
    }
}