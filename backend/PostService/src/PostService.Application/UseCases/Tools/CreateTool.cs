using System.Text.Json;
using PostService.Application.DTOs.Deserialize;
using PostService.Application.DTOs.Request;
using PostService.Application.Exceptions;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Tools.Interfaces;
using PostService.Application.Validations;
using PostService.Domain.Entities;
namespace PostService.Application.UseCases.Tools
{
    public class CreateTool : ICreateTool
    {
        private readonly IMediaFileServices mediaFileServices;
        private readonly IToolsServices toolsServices;
        private readonly ICategoryServices categoryServices;
        private readonly IToolContentServices toolContentServices;
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        public CreateTool(
            IMediaFileServices _mediaFileServices,
            IToolsServices _toolsServices,
            ICategoryServices _categoryServices,
            IToolContentServices _toolContentServices
        )
        {
            this.mediaFileServices = _mediaFileServices;
            this.toolsServices = _toolsServices;
            this.categoryServices = _categoryServices;
            this.toolContentServices = _toolContentServices;
        }
        public async Task ExecuteAsync(ToolRequest request)
        {
            ValidateMainImage(request);
            var toolContents = DeserializeToolContents(request.ToolContents);
            var categories = DeserializeCategories(request.Categories);
            var tool = new Tool(request.Status);
            var mediasToCommit = new List<MediaFile>();
            var mediasToDelete = new List<MediaFile>();
            await ProcessToolContents(tool, toolContents, mediasToCommit, mediasToDelete);
            await ProcessCategories(tool, categories);
            var media = await mediaFileServices.SaveImageAsync(request.ImgFile!, "media/tools");
            tool.AddImgUrl(media!.Path);
            mediasToCommit.Add(media);
            await toolsServices.Save(tool);
            await CommitMedias(mediasToCommit);
            await DeleteMedias(mediasToDelete);
        }
        private static void ValidateMainImage(ToolRequest request)
        {
            if (request.ImgUrl == null)
                throw new ValidationException("Endereço de imagem obrigatório.");
            if (request.ImgFile == null)
                throw new ValidationException("Imagem obrigatória.");
        }
        private static List<ToolContentDeserialize> DeserializeToolContents(string jsonToolContents)
        {
            var toolContentRequest = JsonSerializer.Deserialize<List<ToolContentDeserialize>>(jsonToolContents, JsonOptions);
            if (toolContentRequest is null || toolContentRequest.Count == 0)
                throw new ValidationException("Não é possivel salvar uma ferramenta sem seu conteudo relacionado.");
            return toolContentRequest;
        }
        private static List<CategoryDeserialize> DeserializeCategories(string jsonCategories)
        {
            var categoriesRequest = JsonSerializer.Deserialize<List<CategoryDeserialize>>(jsonCategories, JsonOptions);
            if (categoriesRequest is null || categoriesRequest.Count == 0)
                throw new ValidationException("Não é possivel salvar uma ferramenta sem suas categorias relacionadas.");
            return categoriesRequest;
        }
        private async Task ProcessToolContents(Tool tool, List<ToolContentDeserialize> toolContentRequests, List<MediaFile> mediasToCommit, List<MediaFile> mediasToDelete)
        {
            foreach (var item in toolContentRequests)
            {
                var validationError = ValidationHelper.Validate(item);
                if (validationError.Count > 0)
                {
                    var errors = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                    throw new ValidationException($"Erro ao validar dados: {errors}");
                }
                var toolContent = await this.toolContentServices.FindBy(tc => tc.Slug == item.Slug && tc.LanguageId == item.LanguageId);
                if (toolContent != null)
                    throw new ValidationException("Erro ao validar dados!");
                toolContent = new ToolContent(tool.Id, item.LanguageId, item.Name, item.Title, item.Description, item.Content, item.Slug);
                var toRemoveImages = item.ImagesUrls.Where(image => !item.Content.Contains(image)).ToList();
                foreach (var removeImageUrl in toRemoveImages)
                {
                    var mediaFileContent = await this.mediaFileServices.GetByPath(removeImageUrl);
                    if (mediaFileContent != null)
                        if (!mediasToDelete.Any(m => m.Id == mediaFileContent.Id))
                            mediasToDelete.Add(mediaFileContent);
                    item.ImagesUrls.Remove(removeImageUrl);
                }
                foreach (var addImageUrl in item.ImagesUrls)
                {
                    var mediaFileContent = await this.mediaFileServices.GetByPath(addImageUrl);
                    if (mediaFileContent != null)
                    {
                        if (!mediasToCommit.Any(m => m.Id == mediaFileContent.Id))
                            mediasToCommit.Add(mediaFileContent);
                    }
                }
                toolContent.SetImagesUrls(item.ImagesUrls);
                tool.AddToolContent(toolContent);
            }
        }
        private async Task ProcessCategories(Tool tool, List<CategoryDeserialize> categoriesRequest)
        {
            foreach (var item in categoriesRequest)
            {
                var validationError = ValidationHelper.Validate(item);
                if (validationError.Count > 0)
                {
                    var errors = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                    throw new ValidationException($"Erro ao validar dados: {errors}");
                }
                if (item.Id is not Guid categoryId)
                    throw new ValidationException("O identificador relacionado as categorias são obrigatórios");
                var category = await this.categoryServices.GetById(categoryId);
                if (category == null)
                    throw new NotFoundException("Categoria não encontrada.");
                tool.AddCategory(category);
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