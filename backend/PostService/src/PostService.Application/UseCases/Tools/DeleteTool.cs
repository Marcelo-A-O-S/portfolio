using PostService.Application.Interfaces;
using PostService.Application.UseCases.Tools.Interfaces;
using PostService.Domain.Entities;

namespace PostService.Application.UseCases.Tools
{
    public class DeleteTool : IDeleteTool
    {
        private readonly IMediaFileServices mediaFileServices;
        private readonly IToolsServices toolsServices;
        public DeleteTool(
            IMediaFileServices _mediaFileServices,
            IToolsServices _toolsServices
        )
        {
            this.mediaFileServices = _mediaFileServices;
            this.toolsServices = _toolsServices;
        }
        public async Task ExecuteAsync(Guid Id)
        {
            var tool = await GetTool(Id);
            var mediasToDelete = new List<MediaFile>();
            await ProcessToolImage(tool, mediasToDelete);
            await ProcessToolContents(tool,mediasToDelete);
            await this.toolsServices.DeleteById(tool.Id);
            await DeleteMedias(mediasToDelete);
        }
        private async Task<Tool> GetTool(Guid Id)
        {
            var tool = await this.toolsServices.GetFullDataById(Id);
            if (tool == null)
                throw new Exception("Ferramenta não encontrada.");
            return tool;
        }
        private async Task ProcessToolImage(Tool tool, List<MediaFile> mediasToDelete)
        {
            var mediaImgUrl = await this.mediaFileServices.GetByPath(tool.ImgUrl);
            if (mediaImgUrl != null)
                mediasToDelete.Add(mediaImgUrl);
        }
        private async Task ProcessToolContents(Tool tool, List<MediaFile> mediasToDelete)
        {
            foreach (var toolContent in tool.ToolContents)
            {
                foreach (var imagePath in toolContent.ImagesUrls)
                {
                    var mediaImageContent = await this.mediaFileServices.GetByPath(imagePath);
                    if (mediaImageContent != null)
                        if(!mediasToDelete.Any(md => md.Id == mediaImageContent.Id))
                            mediasToDelete.Add(mediaImageContent);
                }
            }
        }
        private async Task DeleteMedias(List<MediaFile> mediasToDelete)
        {
            foreach (var media in mediasToDelete)
            {
                await this.mediaFileServices.DeleteImageAsync(media);
            }
        }
    }
}