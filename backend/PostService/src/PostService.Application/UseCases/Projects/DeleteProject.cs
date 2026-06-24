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
        private readonly IMediaProjectionServices mediaProjectionServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        public DeleteProject(
            IPostServices _postServices,
            IMediaProjectionServices _mediaProjectionServices,
            IRabbitMQProducer _rabbitMQProducer
        )
        {
            this.postServices = _postServices;
            this.mediaProjectionServices = _mediaProjectionServices;
            this.rabbitMQProducer = _rabbitMQProducer;
        }
        public async Task ExecuteAsync(Guid Id)
        {
            var post = await GetProjectById(Id);
            var mediasToDelete = new List<MediaProjection>();
            await ProcessPostContents(post, mediasToDelete);
            await ProcessTumbnail(post, mediasToDelete);
            await this.postServices.DeleteById(post.Id);
            await DeleteMedias(mediasToDelete);
        }
        public async Task<Post> GetProjectById(Guid Id)
        {
            var post = await postServices.GetFullDataById(Id);
            if (post == null)
                throw new NotFoundException("Projeto não encontrado");
            return post;
        }
        private async Task ProcessPostContents(Post post, List<MediaProjection> mediasToDelete)
        {
            foreach(var postContent in post.PostContents)
            {
                foreach (var media in postContent.Images)
                {
                    mediasToDelete.Add(media);
                    postContent.RemoveImage(media);
                }
                post.RemovePostContent(postContent);
            }
        }
        private async Task ProcessTumbnail(Post post, List<MediaProjection> mediasToDelete)
        {
            var media = await this.mediaProjectionServices.GetByUrl(post.MediaProjection.Url);
            mediasToDelete.Add(media);
        }
        private async Task DeleteMedias(List<MediaProjection> mediasToDelete)
        {
            foreach (var media in mediasToDelete)
            {
                await this.rabbitMQProducer.Publish("PostMediaDeleted", new
                {
                    MediaId = media.MediaId,
                    OwnerType = "Post"
                });
            }
        }
    }
}