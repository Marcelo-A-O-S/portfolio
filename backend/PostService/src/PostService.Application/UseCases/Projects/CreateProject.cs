using PostService.Application.DTOs.Request;
using PostService.Application.Exceptions;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Projects.Interfaces;
using PostService.Application.Validations;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
namespace PostService.Application.UseCases.Projects
{
    public class CreateProject : ICreateProject
    {
        private readonly ICategoryServices categoryServices;
        private readonly IToolsServices toolsServices;
        private readonly IPostContentServices postContentServices;
        private readonly IMediaProjectionServices mediaProjectionServices;
        private readonly IPostServices postServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        public CreateProject(
            ICategoryServices _categoryServices,
            IToolsServices _toolsServices,
            IPostContentServices _postContentServices,
            IMediaProjectionServices _mediaProjectionServices,
            IPostServices _postServices,
            IRabbitMQProducer _rabbitMQProducer
        )
        {
            this.categoryServices = _categoryServices;
            this.toolsServices = _toolsServices;
            this.postContentServices = _postContentServices;
            this.mediaProjectionServices = _mediaProjectionServices;
            this.postServices = _postServices;
            this.rabbitMQProducer = _rabbitMQProducer;
        }
        public async Task ExecuteAsync(PostRequest request)
        {
            ValidateRequest(request);
            var post = new Post(request.Status);
            var mediasToCommit = new List<MediaProjection>();
            var mediasToDelete = new List<MediaProjection>();
            await ProcessPostContents(post, request.PostContents, mediasToCommit, mediasToDelete);
            await ProcessCategories(post, request.Categories);
            await ProcessTools(post, request.Tools);
            await ProcessTumbnail(post, request.Media, mediasToCommit);
            await this.postServices.Save(post);
            await CommitMedias(post.Id, mediasToCommit);
            await DeleteMedias(mediasToDelete);
        }
        private static void ValidateRequest(PostRequest request)
        {
            var validationError = ValidationHelper.Validate(request);
            if (validationError.Count > 0)
            {
                var errors = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {errors}");
            }
        }
        private async Task ProcessCategories(Post post, List<CategoryRequest> categoriesRequest)
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
                post.AddCategory(category);
            }
        }
        private async Task ProcessTools(Post post, List<ToolRequest> toolRequests)
        {
            foreach (var item in toolRequests)
            {
                var validationError = ValidationHelper.Validate(item);
                if (validationError.Count > 0)
                {
                    var errors = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                    throw new ValidationException($"Erro ao validar dados: {errors}");
                }
                if (item.Id is not Guid toolId)
                    throw new ValidationException("O identificador da ferramenta relacionado ao projeto é obrigatório");
                var tool = await this.toolsServices.GetById(toolId);
                if (tool == null)
                    throw new NotFoundException("Ferramenta não encontrada.");
                post.AddTool(tool);
            }
        }
        private async Task ProcessPostContents(Post post, List<PostContentRequest> postContents, List<MediaProjection> mediasToCommit, List<MediaProjection> mediasToDelete)
        {
            foreach (var item in postContents)
            {
                var validationError = ValidationHelper.Validate(item);
                if (validationError.Count > 0)
                {
                    var errors = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                    throw new ValidationException($"Erro ao validar dados: {errors}");
                }
                var postContent = await this.postContentServices.FindBy(pc => pc.Slug == item.Slug && pc.LanguageId == item.LanguageId);
                if (postContent != null)
                    throw new ValidationException("Erro ao validar dados!");
                postContent = new PostContent(post.Id,item.LanguageId,item.Title, item.Description,item.Content, item.Slug);
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
                        var media = new MediaProjection(removeImage.MediaId, removeImage.Url);
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
                        var media = new MediaProjection(addImage.MediaId, addImage.Url);
                        await this.mediaProjectionServices.Save(media);
                        if (!mediasToCommit.Any(m => m.MediaId == media.MediaId))
                        {
                            mediasToCommit.Add(media);
                        }
                        if (!postContent.Images.Any(image => image.MediaId == media.MediaId))
                        {
                            postContent.AddImage(media);
                        }
                    }
                    else
                    {
                        if (!mediasToCommit.Any(m => m.MediaId == mediaContent.MediaId))
                        {
                            mediasToCommit.Add(mediaContent);
                        }
                        if (!postContent.Images.Any(image => image.MediaId == mediaContent.MediaId))
                        {
                            postContent.AddImage(mediaContent);
                        }
                    }
                }
                post.AddPostContent(postContent);
            }
        }
        private async Task ProcessTumbnail(Post post, MediaRequest mediaRequest, List<MediaProjection> mediasToCommit)
        {
            var validationError = ValidationHelper.Validate(mediaRequest);
            if (validationError.Count > 0)
            {
                var errors = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {errors}");
            }
            var mediaContent = await this.mediaProjectionServices.GetByUrl(mediaRequest.Url);
            if (mediaContent != null)
            {
                if (!mediasToCommit.Any(m => m.MediaId == mediaContent.MediaId))
                {
                    mediasToCommit.Add(mediaContent);
                }
                post.SetThumbnail(mediaContent.Id);
                return;
            }
            mediaContent = new MediaProjection(mediaRequest.MediaId, mediaRequest.Url);
            await this.mediaProjectionServices.Save(mediaContent);
            if (!mediasToCommit.Any(m => m.MediaId == mediaContent.MediaId))
            {
                mediasToCommit.Add(mediaContent);
            }
            post.SetThumbnail(mediaContent.Id);
        }
        private async Task CommitMedias(Guid postId, List<MediaProjection> mediasToCommit)
        {
            foreach (var media in mediasToCommit)
            {
                await this.rabbitMQProducer.Publish("PostMediaAttached", new
                {
                    MediaId = media.MediaId,
                    OwnerId = postId,
                    OwnerType = "Post"
                });
            }
        }
        private async Task DeleteMedias(List<MediaProjection> mediasToDelete)
        {
            foreach (var media in mediasToDelete)
            {
                if (media.Id != Guid.Empty)
                {
                    await this.mediaProjectionServices.Delete(media);
                }
                await this.rabbitMQProducer.Publish("PostMediaDeleted", new
                {
                    MediaId = media.MediaId,
                    OwnerType = "Post"
                });
            }
        }
    }
}