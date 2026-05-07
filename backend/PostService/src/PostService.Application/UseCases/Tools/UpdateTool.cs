using System.Text.Json;
using Microsoft.AspNetCore.Http;
using PostService.Application.DTOs.Request;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Tools.Interfaces;
using PostService.Application.Validations;
using PostService.Domain.Entities;

namespace PostService.Application.UseCases.Tools
{
    public class UpdateTool : IUpdateTool
    {
        private readonly IToolsServices toolsServices;
        private readonly IMediaFileServices mediaFileServices;
        private readonly ICategoryServices categoryServices;
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        public UpdateTool(
            IToolsServices _toolsServices,
            IMediaFileServices _mediaFileServices,
            ICategoryServices _categoryServices
        )
        {
            this.toolsServices = _toolsServices;
            this.mediaFileServices = _mediaFileServices;
            this.categoryServices = _categoryServices;
        }
        public async Task ExecuteAsync(Guid Id, ToolRequest toolRequest)
        {
            ValidateToolRequest(toolRequest);
            var toolContents = DeserializeToolContents(toolRequest.ToolContents);
            var categories = DeserializeCategories(toolRequest.Categories);
            var mediasToCommit = new List<MediaFile>();
            var mediasToDelete = new List<MediaFile>();
            var tool = await GetTool(Id);
            await ProcessToolContents(tool, toolContents, mediasToCommit, mediasToDelete);
            await ProcessCategories(tool, categories);
            await UpdateImage(tool, toolRequest.ImgFile, mediasToCommit, mediasToDelete);
            await this.toolsServices.Update(tool);
            await CommitMedias(mediasToCommit);
            await DeleteMedias(mediasToDelete);
        }
        private static void ValidateToolRequest(ToolRequest toolRequest)
        {
            if (toolRequest.ImgUrl == null)
                throw new Exception("Endereço de imagem obrigatório.");
            var validationError = ValidationHelper.Validate(toolRequest);
            if (validationError.Count > 0)
                throw new Exception($"Erro ao validar dados: {validationError}");
        }
        private static List<ToolContentRequest> DeserializeToolContents(string jsonToolContents)
        {
            var toolContentRequest = JsonSerializer.Deserialize<List<ToolContentRequest>>(jsonToolContents, JsonOptions);
            if (toolContentRequest is null || toolContentRequest.Count == 0)
                throw new Exception("Não é possivel salvar uma ferramenta sem seu conteudo relacionado.");
            return toolContentRequest;
        }
        private static List<CategoryRequest> DeserializeCategories(string jsonCategories)
        {
            var categoriesRequest = JsonSerializer.Deserialize<List<CategoryRequest>>(jsonCategories, JsonOptions);
            if (categoriesRequest is null || categoriesRequest.Count == 0)
                throw new Exception("Não é possivel salvar uma ferramenta sem suas categorias relacionadas.");
            return categoriesRequest;
        }
        private async Task<Tool> GetTool(Guid Id)
        {
            var tool = await this.toolsServices.GetForUpdate(Id);
            if (tool == null)
                throw new Exception("Ferramenta não encontrada.");
            return tool;
        }
        private async Task ProcessToolContents(Tool tool, List<ToolContentRequest> toolContentRequests, List<MediaFile> mediasToCommit, List<MediaFile> mediasToDelete)
        {
            var requestToolContentIds = toolContentRequests
                    .Where(c => c.Id.HasValue)
                    .Select(c => c.Id!.Value);
            var removedContents = tool.ToolContents
                .Where(tc => !requestToolContentIds.Contains(tc.Id));
            tool.ValidateToolContents(requestToolContentIds);
            foreach (var item in removedContents)
            {
                foreach (var imageContentRemove in item.ImagesUrls)
                {
                    var imageByDelete = await this.mediaFileServices.GetByPath(imageContentRemove);
                    if (imageByDelete != null)
                        mediasToDelete.Add(imageByDelete);
                }
            }
            foreach (var item in toolContentRequests)
            {
                var validationError = ValidationHelper.Validate(item);
                if (validationError.Count > 0)
                    throw new Exception($"Erro ao validar dados: {validationError}");
                if (item.Id.HasValue)
                {
                    var toolContent = tool.ToolContents.FirstOrDefault(tc => tc.Id == item.Id.Value);
                    if (toolContent == null)
                        throw new Exception("Conteúdo da ferramenta não encontrada.");
                    toolContent.Update(item.LanguageId, item.Name, item.Description, item.Content, item.Slug);
                    await ProcessToolContentImages(item, mediasToCommit, mediasToDelete);
                    toolContent.SetImagesUrls(item.ImagesUrls);
                }
                else
                {
                    var toolContent = new ToolContent(tool.Id, item.LanguageId, item.Name, item.Description, item.Content, item.Slug);
                    await ProcessToolContentImages(item, mediasToCommit, mediasToDelete);
                    toolContent.SetImagesUrls(item.ImagesUrls);
                    tool.AddToolContent(toolContent);
                }
            }
        }
        private async Task ProcessToolContentImages(
                    ToolContentRequest item,
                    List<MediaFile> mediasToCommit,
                    List<MediaFile> mediasToDelete)
        {
            var toRemoveImages = item.ImagesUrls.Where(image => !item.Content.Contains(image)).ToList();
            foreach (var removeImageUrl in toRemoveImages)
            {
                var removeMediaFileContent = await this.mediaFileServices.GetByPath(removeImageUrl);
                if (removeMediaFileContent != null)
                    if (!mediasToDelete.Any(m => m.Id == removeMediaFileContent.Id))
                        mediasToDelete.Add(removeMediaFileContent);
                item.ImagesUrls.Remove(removeImageUrl);
            }
            foreach (var addImageUrl in item.ImagesUrls)
            {
                var addMediaFileContent = await this.mediaFileServices.GetByPath(addImageUrl);
                if (addMediaFileContent != null)
                {
                    if (!mediasToCommit.Any(m => m.Id == addMediaFileContent.Id))
                        mediasToCommit.Add(addMediaFileContent);
                }
            }
        }
        private async Task ProcessCategories(Tool tool, List<CategoryRequest> categoryRequests)
        {
            var requestCategoryIds = categoryRequests
                    .Where(c => c.Id.HasValue)
                    .Select(c => c.Id!.Value);
            tool.ValidateCategories(requestCategoryIds);
            foreach (var item in categoryRequests)
            {
                if (item.Id is not Guid categoryId)
                    throw new Exception("Não é possível atualizar uma categoria sem seu identificador");
                var exists = tool.Categories.Any(c => c.Id == categoryId);
                if (exists) continue;
                var category = await this.categoryServices.GetById(categoryId);
                if (category == null)
                    throw new Exception("Conteúdo da categoria não encontrada.");
                tool.AddCategory(category);
            }
        }
        private async Task UpdateImage(Tool tool, IFormFile? imgFile, List<MediaFile> mediasToCommit, List<MediaFile> mediasToDelete)
        {
            if (imgFile != null)
            {
                var mediaFileTool = await this.mediaFileServices.SaveImageAsync(imgFile, "media/tools");
                if (mediaFileTool is null)
                    throw new Exception("Erro ao atualizar a imagem.");
                var searchMediaToolCurrent = await this.mediaFileServices.GetByPath(tool.ImgUrl);
                if (searchMediaToolCurrent != null)
                    mediasToDelete.Add(searchMediaToolCurrent);
                tool.AddImgUrl(mediaFileTool.Path);
                mediasToCommit.Add(mediaFileTool);
            }
        }
        private async Task CommitMedias(List<MediaFile> mediasToCommit)
        {
            foreach (var media in mediasToCommit)
            {
                media.Commit();
                await this.mediaFileServices.Update(media);
            }
        }
        private async Task DeleteMedias(List<MediaFile> mediasToDelete)
        {
            foreach (var mediaDelete in mediasToDelete)
            {
                await this.mediaFileServices.DeleteImageAsync(mediaDelete);
            }
        }
    }
}