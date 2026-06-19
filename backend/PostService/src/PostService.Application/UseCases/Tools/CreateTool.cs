using System.Collections.Generic;
using System.Collections.Generic;
using System.Text.Json;
using PostService.Application.DTOs.Deserialize;
using PostService.Application.DTOs.Request;
using PostService.Application.Exceptions;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Tools.Interfaces;
using PostService.Application.Validations;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
namespace PostService.Application.UseCases.Tools
{
    public class CreateTool : ICreateTool
    {
        private readonly IMediaProjectionServices mediaProjectionServices;
        private readonly IToolsServices toolsServices;
        private readonly ICategoryServices categoryServices;
        private readonly IToolContentServices toolContentServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        public CreateTool(
            IMediaProjectionServices _mediaProjectionServices,
            IToolsServices _toolsServices,
            ICategoryServices _categoryServices,
            IToolContentServices _toolContentServices,
            IRabbitMQProducer _rabbitMQProducer
        )
        {
            this.mediaProjectionServices = _mediaProjectionServices;
            this.toolsServices = _toolsServices;
            this.categoryServices = _categoryServices;
            this.toolContentServices = _toolContentServices;
            this.rabbitMQProducer = _rabbitMQProducer;
        }
        public async Task ExecuteAsync(ToolRequest request)
        {
            ValidateRequest(request);
            var mediasToDelete = new List<MediaProjection>();
            var mediasToCommit = new List<MediaProjection>();
            var tool = new Tool(request.Status);
            await ProcessToolContents(tool, request.ToolContents, mediasToCommit, mediasToDelete);
            await ProcessCategories(tool, request.Categories);
            await ProcessTumbnail(tool, request.Media);
            await this.toolsServices.Save(tool);
            await CommitMedias(tool.Id, mediasToCommit);
            await DeleteMedias(mediasToDelete);
        }
        private static void ValidateRequest(ToolRequest request)
        {
            var validationError = ValidationHelper.Validate(request);
            if (validationError.Count > 0)
            {
                var errors = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {errors}");
            }
        }
        private async Task ProcessToolContents(Tool tool, List<ToolContentRequest> toolContentRequests, List<MediaProjection> mediasToCommit, List<MediaProjection> mediasToDelete)
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
                var toRemoveImages = item.Images.Where(image => !item.Content.Contains(image.Url)).ToList();
                foreach (var removeImage in toRemoveImages)
                {
                    var mediaContent = await this.mediaProjectionServices.GetByUrl(removeImage.Url);
                    if (mediaContent != null)
                    {
                        if (!mediasToDelete.Any(m => m.MediaId == mediaContent.MediaId))
                        {
                            mediasToDelete.Add(mediaContent);
                            continue;
                        }
                    }
                    else
                    {
                        var media = new MediaProjection(removeImage.Id, removeImage.Url);
                        if (!mediasToDelete.Any(m => m.MediaId == media.MediaId))
                        {
                            mediasToDelete.Add(media);
                            continue;
                        }
                    }
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
                            toolContent.AddImageUrl(media);
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
                            toolContent.AddImageUrl(mediaContent);
                        }
                    }
                }
                tool.AddToolContent(toolContent);
            }
        }
        private async Task ProcessCategories(Tool tool, List<CategoryRequest> categoriesRequest)
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