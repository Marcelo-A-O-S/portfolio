using System.Text.Json;
using PostService.Application.DTOs.Deserialize;
using PostService.Application.DTOs.Request;
using PostService.Application.Exceptions;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Projects.Interfaces;
using PostService.Application.Validations;
using PostService.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using PostService.Domain.Interfaces;

namespace PostService.Application.UseCases.Projects
{
    public class UpdateProject : IUpdateProject
    {
        private readonly IPostServices postServices;
        private readonly ICategoryServices categoryServices;
        private readonly IToolsServices toolsServices;
        private readonly IMediaProjectionServices mediaProjectionServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        public UpdateProject(
            IPostServices _postServices,
            ICategoryServices _categoryServices,
            IToolsServices _toolsServices,
            IMediaProjectionServices _mediaProjectionServices,
            IRabbitMQProducer _rabbitMQProducer
        )
        {
            this.postServices = _postServices;
            this.categoryServices = _categoryServices;
            this.toolsServices = _toolsServices;
            this.mediaProjectionServices = _mediaProjectionServices;
            this.rabbitMQProducer = _rabbitMQProducer;
        }
        public async Task ExecuteAsync(Guid Id, PostRequest request)
        {
            ValidateRequest(request);
            var post = await GetPostById(Id);
            var mediasToCommit = new List<MediaProjection>();
            var mediasToDelete = new List<MediaProjection>();
            await ProcessCategories(post, request.Categories);
            await ProcessTools(post, request.Tools);
            await ProcessPostContents(post, request.PostContents, mediasToCommit, mediasToDelete);
            await ProcessTumbnail(post, request.Media, mediasToCommit, mediasToDelete);
            await this.postServices.Update(post);
            await CommitMedias(Id,mediasToCommit);
            await DeleteMedias(mediasToDelete);
        }
        private static void ValidateRequest(PostRequest request)
        {
            var validationError = ValidationHelper.Validate(request);
            if (validationError.Count > 0)
                throw new ValidationException($"Erro ao validar dados: {validationError}");
        }
        private async Task<Post> GetPostById(Guid Id)
        {
            var post = await this.postServices.GetForUpdate(Id);
            if (post == null)
                throw new NotFoundException("Projeto não encontrado.");
            return post;
        }
        private async Task ProcessCategories(Post post, List<CategoryRequest> categoriesRequest)
        {
            var requestCategoryIds = categoriesRequest
                    .Where(c => c.Id.HasValue)
                    .Select(c => c.Id!.Value);
            post.ValidateCategories(requestCategoryIds);
            foreach (var item in categoriesRequest)
            {
                if (item.Id is not Guid categoryId)
                    throw new ValidationException("Não é possível atualizar uma categoria sem seu identificador");
                var exists = post.Categories.Any(c => c.Id == categoryId);
                if (exists) continue;
                var category = await this.categoryServices.GetById(categoryId);
                if (category == null)
                    throw new NotFoundException("Conteúdo da categoria não encontrada.");
                post.AddCategory(category);
            }
        }
        private async Task ProcessTools(Post post, List<ToolRequest> toolsRequest)
        {
            var requestToolIds = toolsRequest
                .Where(pc => pc.Id.HasValue)
                .Select(pc => pc.Id!.Value);
            post.ValidateTools(requestToolIds);
            foreach (var item in toolsRequest)
            {
                if (item.Id is not Guid toolId)
                    throw new ValidationException("O identificador da ferramenta é obrigatório");
                var exists = post.Tools.Any(t => t.Id == item.Id);
                if (exists) continue;
                var tool = await this.toolsServices.GetById(toolId);
                if (tool == null)
                    throw new NotFoundException("A ferramenta não foi encontrada.");
                post.AddTool(tool);
            }
        }
        private async Task ProcessPostContents(Post post, List<PostContentRequest> postContentRequests, List<MediaProjection> mediasToCommit, List<MediaProjection> mediasToDelete)
        {
            var requestPostContentIds = postContentRequests
                .Where(pc => pc.Id.HasValue)
                .Select(pc => pc.Id!.Value);
            var removedContents = post.PostContents
                .Where(pc => !requestPostContentIds.Contains(pc.Id));
            post.ValidatePostContents(requestPostContentIds);  
            foreach(var item in removedContents)
            {
                foreach (var removeImage in item.Images)
                {
                    var image = await this.mediaProjectionServices.FindBy(img => img.MediaId == removeImage.MediaId && img.Id == removeImage.Id);
                    if(image != null)
                    {
                        mediasToDelete.Add(image);
                    }
                }
                post.RemovePostContent(item);
            }
            foreach(var item in postContentRequests)
            {
                var validationError = ValidationHelper.Validate(item);
                if (validationError.Count > 0)
                    throw new ValidationException($"Erro ao validar dados: {validationError}");
                if (item.Id.HasValue)
                {
                    var postContent = post.PostContents.FirstOrDefault(tc => tc.Id == item.Id.Value);
                    if (postContent == null)
                        throw new NotFoundException("Conteúdo da ferramenta não encontrada.");
                    postContent.Update(item.LanguageId, item.Title, item.Description, item.Content, item.Slug);
                    await ProcessPostContentImages(postContent, item, mediasToCommit, mediasToDelete);
                }
                else
                {
                    var postContent = new PostContent(post.Id, item.LanguageId, item.Title, item.Description, item.Content, item.Slug);
                    await ProcessPostContentImages(postContent, item, mediasToCommit, mediasToDelete);
                }
            }
        }
        private async Task ProcessPostContentImages(PostContent postContent, PostContentRequest item , List<MediaProjection> mediasToCommit, List<MediaProjection> mediasToDelete)
        {
            var toRemoveImages = postContent.Images.Where(media => !item.Content.Contains(media.Url)).ToList();
            foreach(var removeImage in toRemoveImages)
            {
                var mediaContent = await this.mediaProjectionServices.GetByUrl(removeImage.Url);
                if(mediaContent != null)
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
                postContent.RemoveImage(removeImage);
            }
            var toAddImages = item.Images.Where(image => item.Content.Contains(image.Url)).ToList();
            foreach(var addImage in toAddImages)
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
        }
        private async Task ProcessTumbnail(Post post, MediaRequest mediaRequest, List<MediaProjection> mediasToCommit, List<MediaProjection> mediasToDelete)
        {
            var validationError = ValidationHelper.Validate(mediaRequest);
            if (validationError.Count > 0)
            {
                var errors = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {errors}");
            }
            if (post.MediaProjectionId == mediaRequest.Id)
                return;
            if (!mediasToDelete.Any(m => m.MediaId == post.MediaProjection.MediaId))
            {
                mediasToDelete.Add(post.MediaProjection);
            }
            var mediaContent = await this.mediaProjectionServices.GetByUrl(mediaRequest.Url);
            if(mediaContent != null)
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
                if(media.Id != Guid.Empty)
                {
                    await this.mediaProjectionServices.Delete(media);
                }
                await this.rabbitMQProducer.Publish("PostMediaDeleted",new
                {
                    MediaId = media.MediaId,
                    OwnerType = "Post"
                });
            }
        }
        // private async Task UpdateImage(Post post, IFormFile? imgFile, List<MediaFile> mediasToCommit, List<MediaFile> mediasToDelete)
        // {
        //     if (imgFile != null)
        //     {
        //         var mediaFileTool = await this.mediaFileServices.SaveImageAsync(imgFile, "media/tools");
        //         if (mediaFileTool is null)
        //             throw new Exception("Erro ao atualizar a imagem.");
        //         var searchMediaToolCurrent = await this.mediaFileServices.GetByPath(post.ImgUrl);
        //         if (searchMediaToolCurrent != null)
        //             mediasToDelete.Add(searchMediaToolCurrent);
        //         post.AddImgUrl(mediaFileTool.Path);
        //         mediasToCommit.Add(mediaFileTool);
        //     }
        // }
    }
}