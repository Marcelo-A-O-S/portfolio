using PostService.Application.Exceptions;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Tools.Interfaces;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using System.Text.Json;
namespace PostService.Application.UseCases.Tools
{
    public class DeleteTool : IDeleteTool
    {
        private readonly IMediaProjectionServices mediaProjectionServices;
        private readonly IToolsServices toolsServices;
        private readonly IOutboxMessageServices outboxMessageServices;
        private readonly IUnitOfWork unitOfWork;
        public DeleteTool(
            IMediaProjectionServices _mediaProjectionServices,
            IToolsServices _toolsServices,
            IOutboxMessageServices _outboxMessageServices,
            IUnitOfWork _unitOfWork
        )
        {
            this.mediaProjectionServices = _mediaProjectionServices;
            this.toolsServices = _toolsServices;
            this.outboxMessageServices = _outboxMessageServices;
            this.unitOfWork = _unitOfWork;
        }
        public async Task ExecuteAsync(Guid Id)
        {
            var tool = await GetTool(Id);
            var mediasToDelete = new List<MediaProjection>();
            await ProcessToolContents(tool, mediasToDelete);
            await ProcessTumbnail(tool, mediasToDelete);
            await this.unitOfWork.BeginAsync();
            try
            {
                await this.toolsServices.DeleteById(tool.Id);
                await DeleteMedias(Id,mediasToDelete);
                await this.unitOfWork.CommitAsync();
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
        }
        private async Task<Tool> GetTool(Guid Id)
        {
            var tool = await this.toolsServices.GetFullDataById(Id);
            if (tool == null)
                throw new NotFoundException("Ferramenta não encontrada.");
            return tool;
        }
        private async Task ProcessToolContents(Tool tool, List<MediaProjection> mediasToDelete)
        {
            foreach (var toolContent in tool.ToolContents)
            {
                foreach (var media in toolContent.Images)
                {
                    mediasToDelete.Add(media);
                    toolContent.RemoveImage(media);
                }
                tool.RemoveToolContent(toolContent);
            }
        }
        private async Task ProcessTumbnail(Tool tool, List<MediaProjection> mediasToDelete)
        {
            var media = await this.mediaProjectionServices.GetByUrl(tool.MediaProjection.Url);
            mediasToDelete.Add(media);
        }
        private async Task DeleteMedias(Guid toolId, List<MediaProjection> mediasToDelete)
        {
            foreach (var media in mediasToDelete)
            {
                var outboxMessage = new OutboxMessage(
                    "ToolMediaDeleted",
                    JsonSerializer.Serialize(new
                    {
                        MediaId = media.MediaId,
                        OwnerId = toolId,
                        OwnerType = "Tool"
                    })
                );
                await this.outboxMessageServices.Save(outboxMessage);
            }
        }
    }
}