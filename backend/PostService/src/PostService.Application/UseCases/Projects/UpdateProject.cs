using System.Text.Json;
using PostService.Application.DTOs.Deserialize;
using PostService.Application.DTOs.Request;
using PostService.Application.Exceptions;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Projects.Interfaces;
using PostService.Application.Validations;
using PostService.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace PostService.Application.UseCases.Projects
{
    public class UpdateProject : IUpdateProject
    {
        private readonly IPostServices postServices;
        private readonly ICategoryServices categoryServices;
        private readonly IToolsServices toolsServices;
        private readonly IMediaFileServices mediaFileServices;
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        public UpdateProject(
            IPostServices _postServices,
            ICategoryServices _categoryServices,
            IToolsServices _toolsServices,
            IMediaFileServices _mediaFileServices
        )
        {
            this.postServices = _postServices;
            this.categoryServices = _categoryServices;
            this.toolsServices = _toolsServices;
            this.mediaFileServices = _mediaFileServices;
        }
        public async Task ExecuteAsync(Guid Id, PostRequest postRequest)
        {
            ValidatePostRequest(postRequest);
            var tools = DeserializeTools(postRequest.Tools);
            var categories = DeserializeCategories(postRequest.Categories);
            var postContents = DeserializePostContents(postRequest.PostContents);
            var mediasToCommit = new List<MediaFile>();
            var mediasToDelete = new List<MediaFile>();
            var post = await GetPostById(Id);
            await ProcessCategories(post, categories);
            await ProcessTools(post, tools);
            await ProcessPostContents(post, postContents, mediasToCommit, mediasToDelete);
            await UpdateImage(post, postRequest.ImgFile, mediasToCommit, mediasToDelete);
            await this.postServices.Update(post);
            await CommitMedias(mediasToCommit);
            await DeleteMedias(mediasToDelete);
        }
        private static void ValidatePostRequest(PostRequest postRequest)
        {
            if (postRequest.ImgUrl == null)
                throw new ValidationException("Endereço de imagem obrigatório.");
            var validationError = ValidationHelper.Validate(postRequest);
            if (validationError.Count > 0)
                throw new ValidationException($"Erro ao validar dados: {validationError}");
        }
        private static List<ToolDeserialize> DeserializeTools(string jsonTools)
        {
            var toolsRequest = JsonSerializer.Deserialize<List<ToolDeserialize>>(jsonTools, JsonOptions);
            if (toolsRequest is null || toolsRequest.Count == 0)
                throw new ValidationException("Não é possivel salvar um projeto sem suas ferramentas relacionadas");
            return toolsRequest;
        }
        private static List<CategoryDeserialize> DeserializeCategories(string jsonCategories)
        {
            var categoriesRequest = JsonSerializer.Deserialize<List<CategoryDeserialize>>(jsonCategories, JsonOptions);
            if (categoriesRequest is null || categoriesRequest.Count == 0)
                throw new ValidationException("Não é possivel salvar um projeto sem suas categorias relacionadas.");
            return categoriesRequest;
        }
        private static List<PostContentDeserialize> DeserializePostContents(string jsonPostContents)
        {
            var postContentRequests = JsonSerializer.Deserialize<List<PostContentDeserialize>>(jsonPostContents, JsonOptions);
            if (postContentRequests is null || postContentRequests.Count == 0)
                throw new ValidationException("Não é possivel salvar um projeto sem seu conteudo relacionadas.");
            return postContentRequests;
        }
        private async Task<Post> GetPostById(Guid Id)
        {
            var post = await this.postServices.GetForUpdate(Id);
            if (post == null)
                throw new NotFoundException("Projeto não encontrado.");
            return post;
        }
        private async Task ProcessCategories(Post post, List<CategoryDeserialize> categoriesRequest)
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
        private async Task ProcessTools(Post post, List<ToolDeserialize> toolsRequest)
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
        private async Task ProcessPostContents(Post post, List<PostContentDeserialize> postContents, List<MediaFile> mediasToCommit, List<MediaFile> mediasToDelete)
        {
            var requestPostContentIds = postContents
                .Where(pc => pc.Id.HasValue)
                .Select(pc => pc.Id!.Value);
            var removedContents = post.PostContents
                .Where(pc => !requestPostContentIds.Contains(pc.Id));
            post.ValidatePostContents(requestPostContentIds);
            foreach (var item in removedContents)
            {
                foreach (var imageContentRemove in item.ImagesUrls)
                {
                    var imageByDelete = await this.mediaFileServices.GetByPath(imageContentRemove);
                    if (imageByDelete != null)
                        mediasToDelete.Add(imageByDelete);
                }
            }
            foreach (var item in postContents)
            {
                var validationError = ValidationHelper.Validate(item);
                if (validationError.Count > 0)
                {
                    var errors = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                    throw new ValidationException($"Erro ao validar dados: {errors}");
                }
                if (item.Id.HasValue)
                {
                    var postContent = post.PostContents.FirstOrDefault(pc => pc.Id == item.Id.Value);
                    if (postContent == null)
                        throw new NotFoundException("Conteudo do projeto não encontrado.");
                    postContent.Update(item.LanguageId, item.Title, item.Description, item.Content, item.Slug);
                    await ProcessPostContentImages(item, mediasToCommit, mediasToDelete);
                    postContent.SetImagesUrls(item.ImagesUrls);
                }
                else
                {
                    var postContent = new PostContent(post.Id, item.LanguageId, item.Title, item.Description, item.Content, item.Slug);
                    await ProcessPostContentImages(item, mediasToCommit, mediasToDelete);
                    postContent.SetImagesUrls(item.ImagesUrls);
                    post.AddPostContent(postContent);
                }
            }
        }
        private async Task ProcessPostContentImages(PostContentDeserialize item, List<MediaFile> mediasToCommit, List<MediaFile> mediasToDelete)
        {
            var toRemoveImages = item.ImagesUrls.Where(image => !item.Content.Contains(image)).ToList();
            foreach (var removeImageUrl in toRemoveImages)
            {
                var removeMediaFileContent = await this.mediaFileServices.GetByPath(removeImageUrl);
                if (removeMediaFileContent != null)
                    if (!mediasToDelete.Any(m => m.Id == removeMediaFileContent.Id))
                        mediasToDelete.Add(removeMediaFileContent);
                item.ImagesUrls.Remove(removeImageUrl);
            }
            foreach (var addImageUrl in item.ImagesUrls)
            {
                var addMediaFileContent = await this.mediaFileServices.GetByPath(addImageUrl);
                if (addMediaFileContent != null)
                {
                    if (!mediasToCommit.Any(m => m.Id == addMediaFileContent.Id))
                        mediasToCommit.Add(addMediaFileContent);
                }
            }
        }
        private async Task UpdateImage(Post post, IFormFile? imgFile, List<MediaFile> mediasToCommit, List<MediaFile> mediasToDelete)
        {
            if (imgFile != null)
            {
                var mediaFileTool = await this.mediaFileServices.SaveImageAsync(imgFile, "media/tools");
                if (mediaFileTool is null)
                    throw new Exception("Erro ao atualizar a imagem.");
                var searchMediaToolCurrent = await this.mediaFileServices.GetByPath(post.ImgUrl);
                if (searchMediaToolCurrent != null)
                    mediasToDelete.Add(searchMediaToolCurrent);
                post.AddImgUrl(mediaFileTool.Path);
                mediasToCommit.Add(mediaFileTool);
            }
        }
        private async Task CommitMedias(List<MediaFile> mediasToCommit)
        {
            foreach (var media in mediasToCommit)
            {
                media.Commit();
                await this.mediaFileServices.Update(media);
            }
        }
        private async Task DeleteMedias(List<MediaFile> mediasToDelete)
        {
            foreach (var mediaDelete in mediasToDelete)
            {
                await this.mediaFileServices.DeleteImageAsync(mediaDelete);
            }
        }
    }
}