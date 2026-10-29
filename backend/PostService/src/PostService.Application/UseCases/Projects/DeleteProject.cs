using System.Text.Json;
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
        private readonly IOutboxMessageServices outboxMessageServices;
        private readonly IUnitOfWork unitOfWork;
        public DeleteProject(
            IPostServices _postServices,
            IMediaProjectionServices _mediaProjectionServices,
            IOutboxMessageServices _outboxMessageServices,
            IUnitOfWork _unitOfWork
        )
        {
            this.postServices = _postServices;
            this.mediaProjectionServices = _mediaProjectionServices;
            this.outboxMessageServices = _outboxMessageServices;
            this.unitOfWork = _unitOfWork;
        }
        public async Task ExecuteAsync(Guid Id)
        {
            var post = await GetProjectById(Id);
            var mediasToDelete = new List<MediaProjection>();
            await ProcessPostContents(post, mediasToDelete);
            await ProcessTumbnail(post, mediasToDelete);
            await this.unitOfWork.BeginAsync();
            try
            {
                await this.postServices.DeleteById(post.Id);
                var outboxMessage = new OutboxMessage(
                        "PostDeleted",
                        JsonSerializer.Serialize(new
                        {
                            PostId = Id
                        })
                    );
                await this.outboxMessageServices.Save(outboxMessage);
                await DeleteMedias(mediasToDelete);
                await this.unitOfWork.CommitAsync();
            }
            catch
            {
                await unitOfWork.RollbackAsync();
                throw;
            }

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
            foreach (var postContent in post.PostContents)
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
            if (media != null)
                mediasToDelete.Add(media);
        }
        private async Task DeleteMedias(List<MediaProjection> mediasToDelete)
        {
            foreach (var media in mediasToDelete)
            {
                var outboxMessage = new OutboxMessage(
                        "PostMediaDeleted",
                        JsonSerializer.Serialize(new
                        {
                            MediaId = media.MediaId,
                            OwnerType = "Post"
                        })
                    );
                await this.outboxMessageServices.Save(outboxMessage);
            }
        }
    }
}