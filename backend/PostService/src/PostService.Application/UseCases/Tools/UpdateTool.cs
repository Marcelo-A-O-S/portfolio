using PostService.Application.DTOs.Request;
using PostService.Application.Exceptions;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Tools.Interfaces;
using PostService.Application.Validations;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
namespace PostService.Application.UseCases.Tools
{
    public class UpdateTool : IUpdateTool
    {
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly IToolsServices toolsServices;
        private readonly IMediaProjectionServices mediaProjectionServices;
        private readonly ICategoryServices categoryServices;
        public UpdateTool(
            IToolsServices _toolsServices,
            IMediaProjectionServices _mediaProjectionServices,
            ICategoryServices _categoryServices,
            IRabbitMQProducer _rabbitMQProducer
        )
        {
            this.toolsServices = _toolsServices;
            this.mediaProjectionServices = _mediaProjectionServices;
            this.categoryServices = _categoryServices;
            this.rabbitMQProducer = _rabbitMQProducer;
        }
        public async Task ExecuteAsync(Guid Id, ToolRequest request)
        {
            ValidateRequest(request);
            var tool = await GetTool(Id);
            var mediasToCommit = new List<MediaProjection>();
            var mediasToDelete = new List<MediaProjection>();
            await ProcessToolContents(tool, request.ToolContents, mediasToCommit, mediasToDelete);
            await ProcessCategories(tool, request.Categories);
            await ProcessTumbnail(tool, request.Media);
            await this.toolsServices.Update(tool);
            await CommitMedias(tool.Id, mediasToCommit);
            await DeleteMedias(mediasToDelete);
        }
        private static void ValidateRequest(ToolRequest toolRequest)
        {
            var validationError = ValidationHelper.Validate(toolRequest);
            if (validationError.Count > 0)
            {
                var errors = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {errors}");
            }
        }
        private async Task<Tool> GetTool(Guid Id)
        {
            var tool = await this.toolsServices.GetForUpdate(Id);
            if (tool == null)
                throw new NotFoundException("Ferramenta não encontrada.");
            return tool;
        }
        private async Task ProcessToolContents(Tool tool, List<ToolContentRequest> toolContentRequests, List<MediaProjection> mediasToCommit, List<MediaProjection> mediasToDelete)
        {
            var requestToolContentIds = toolContentRequests
                .Where(c => c.Id.HasValue)
                .Select(c => c.Id!.Value);
            var removedContents = tool.ToolContents
                .Where(tc => !requestToolContentIds.Contains(tc.Id));
            tool.ValidateToolContents(requestToolContentIds);
            foreach (var item in removedContents)
            {
                foreach (var removeImage in item.Images)
                {
                    var image = await this.mediaProjectionServices.FindBy(img => img.MediaId == removeImage.MediaId && img.Id == removeImage.Id);
                    if(image != null)
                    {
                        mediasToDelete.Add(image);
                    }
                }
                tool.RemoveToolContent(item);
            }
            foreach (var item in toolContentRequests)
            {
                var validationError = ValidationHelper.Validate(item);
                if (validationError.Count > 0)
                    throw new ValidationException($"Erro ao validar dados: {validationError}");
                if (item.Id.HasValue)
                {
                    var toolContent = tool.ToolContents.FirstOrDefault(tc => tc.Id == item.Id.Value);
                    if (toolContent == null)
                        throw new NotFoundException("Conteúdo da ferramenta não encontrada.");
                    toolContent.Update(item.LanguageId, item.Name, item.Title, item.Description, item.Content, item.Slug);
                    await ProcessToolContentImages(toolContent, item, mediasToCommit, mediasToDelete);
                }
                else
                {
                    var toolContent = new ToolContent(tool.Id, item.LanguageId, item.Name, item.Title, item.Description, item.Content, item.Slug);
                    await ProcessToolContentImages(toolContent, item, mediasToCommit, mediasToDelete);
                    tool.AddToolContent(toolContent);
                }
            }
        }
        private async Task ProcessToolContentImages(ToolContent toolContent,ToolContentRequest item, List<MediaProjection> mediasToCommit, List<MediaProjection> mediasToDelete)
        {
            var toRemoveImages = toolContent.Images.Where(media => !item.Content.Contains(media.Url)).ToList();
            foreach (var removeImage in toRemoveImages)
            {
                var mediaContent = await this.mediaProjectionServices.GetByUrl(removeImage.Url);
                if (mediaContent != null)
                {
                    if (!mediasToDelete.Any(m => m.MediaId == mediaContent.MediaId))
                    {
                        mediasToDelete.Add(mediaContent);
                    }
                }
                else
                {
                    var media = new MediaProjection(removeImage.Id, removeImage.Url);
                    if (!mediasToDelete.Any(m => m.MediaId == media.MediaId))
                    {
                        mediasToDelete.Add(media);
                    }
                }
                toolContent.RemoveImage(removeImage);
            }
            var toAddImages = item.Images.Where(image => item.Content.Contains(image.Url)).ToList();
            foreach (var addImage in toAddImages)
            {
                var mediaContent = await this.mediaProjectionServices.GetByUrl(addImage.Url);
                if (mediaContent == null)
                {
                    var media = new MediaProjection(addImage.Id, addImage.Url);
                    await this.mediaProjectionServices.Save(media);
                    if (!mediasToCommit.Any(m => m.MediaId == media.MediaId))
                    {
                        mediasToCommit.Add(media);
                    }
                    if (!toolContent.Images.Any(image => image.MediaId == media.MediaId))
                    {
                        toolContent.AddImage(media);
                    }
                }
                else
                {
                    if (!mediasToCommit.Any(m => m.MediaId == mediaContent.MediaId))
                    {
                        mediasToCommit.Add(mediaContent);
                    }
                    if (!toolContent.Images.Any(image => image.MediaId == mediaContent.MediaId))
                    {
                        toolContent.AddImage(mediaContent);
                    }
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
                    throw new ValidationException("Não é possível atualizar uma categoria sem seu identificador");
                var exists = tool.Categories.Any(c => c.Id == categoryId);
                if (exists) continue;
                var category = await this.categoryServices.GetById(categoryId);
                if (category == null)
                    throw new NotFoundException("Conteúdo da categoria não encontrada.");
                tool.AddCategory(category);
            }
        }
        private async Task ProcessTumbnail(Tool tool, MediaRequest mediaRequest)
        {
            var validationError = ValidationHelper.Validate(mediaRequest);
            if (validationError.Count > 0)
            {
                var errors = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {errors}");
            }
            var mediaContent = await this.mediaProjectionServices.GetByUrl(mediaRequest.Url);
            if(mediaContent != null)
            {
                tool.SetThumbnail(mediaContent.Id);
                return;
            }
            mediaContent = new MediaProjection(mediaRequest.Id, mediaRequest.Url);
            await this.mediaProjectionServices.Save(mediaContent);
            tool.SetThumbnail(mediaContent.Id);
        }
        private async Task CommitMedias(Guid toolId, List<MediaProjection> mediasToCommit)
        {
            foreach (var media in mediasToCommit)
            {
                await this.rabbitMQProducer.Publish("ToolMediaAttached", new
                {
                    MediaId = media.MediaId,
                    OwnerId = toolId,
                    OwnerType = "Tool"
                });
            }
        }
        private async Task DeleteMedias(List<MediaProjection> mediasToDelete)
        {
            foreach (var media in mediasToDelete)
            {
                if(media.Id != Guid.Empty)
                {
                    await this.mediaProjectionServices.Delete(media);
                }
                await this.rabbitMQProducer.Publish("ToolMediaDeleted",new
                {
                    MediaId = media.MediaId,
                    OwnerType = "Tool"
                });
            }
        }
    }
}