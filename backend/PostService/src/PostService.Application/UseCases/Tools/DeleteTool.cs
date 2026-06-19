using PostService.Application.Exceptions;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Tools.Interfaces;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;

namespace PostService.Application.UseCases.Tools
{
    public class DeleteTool : IDeleteTool
    {
        private readonly IMediaProjectionServices mediaProjectionServices;
        private readonly IToolsServices toolsServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        public DeleteTool(
            IMediaProjectionServices _mediaProjectionServices,
            IToolsServices _toolsServices,
            IRabbitMQProducer _rabbitMQProducer
        )
        {
            this.mediaProjectionServices = _mediaProjectionServices;
            this.toolsServices = _toolsServices;
            this.rabbitMQProducer = _rabbitMQProducer;
        }
        public async Task ExecuteAsync(Guid Id)
        {
            var tool = await GetTool(Id);
            var mediasToDelete = new List<MediaProjection>();
            await ProcessToolContents(tool, mediasToDelete);
            await ProcessTumbnail(tool, mediasToDelete);
            await this.toolsServices.DeleteById(tool.Id);
            await DeleteMedias(mediasToDelete);
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
        private async Task DeleteMedias(List<MediaProjection> mediasToDelete)
        {
            foreach (var media in mediasToDelete)
            {
                await this.rabbitMQProducer.Publish("ToolMediaDeleted", new
                {
                    MediaId = media.MediaId,
                    OwnerType = "Tool"
                });
            }
        }
    }
}