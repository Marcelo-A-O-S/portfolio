using PostService.Application.Exceptions;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Projects.Interfaces;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
namespace PostService.Application.UseCases.Projects
{
    public class DeleteProject : IDeleteProject
    {
        private readonly IPostServices postServices;
        private readonly IMediaFileServices mediaFileServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        public DeleteProject(
            IPostServices _postServices,
            IMediaFileServices _mediaFileServices,
            IRabbitMQProducer _rabbitMQProducer
        )
        {
            this.postServices = _postServices;
            this.mediaFileServices = _mediaFileServices;
            this.rabbitMQProducer = _rabbitMQProducer;
        }
        public async Task ExecuteAsync(Guid Id)
        {
            var post = await GetProjectById(Id);
            var mediasToDelete = new List<MediaFile>();
            await ProcessPostImage(post, mediasToDelete);
            await ProcessPostContentImages(post, mediasToDelete);
            await postServices.DeleteById(post.Id);
            await DeleteMedias(mediasToDelete);
            await this.rabbitMQProducer.Publish("PostDeleted", new { PostId = Id });
        }
        public async Task<Post> GetProjectById(Guid Id)
        {
            var post = await postServices.GetFullDataById(Id);
            if (post == null)
                throw new NotFoundException("Projeto não encontrado");
            return post;
        }
        public async Task ProcessPostImage(Post post, List<MediaFile> mediasToDelete)
        {
            var mediaImgUrl = await this.mediaFileServices.GetByPath(post.ImgUrl);
            if (mediaImgUrl != null)
                mediasToDelete.Add(mediaImgUrl);
        }
        public async Task ProcessPostContentImages(Post post, List<MediaFile> mediasToDelete)
        {
            foreach (var postContent in post.PostContents)
            {
                foreach (var imagePath in postContent.ImagesUrls)
                {
                    var mediaImageContent = await this.mediaFileServices.GetByPath(imagePath);
                    if (mediaImageContent != null)
                        if (!mediasToDelete.Any(md => md.Id == mediaImageContent.Id))
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