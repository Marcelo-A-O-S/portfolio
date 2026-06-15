using System.Text.Json;
using PostService.Application.DTOs.Deserialize;
using PostService.Application.DTOs.Request;
using PostService.Application.Exceptions;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Projects.Interfaces;
using PostService.Application.Validations;
using PostService.Domain.Entities;

namespace PostService.Application.UseCases.Projects
{
    public class CreateProject : ICreateProject
    {
        private readonly ICategoryServices categoryServices;
        private readonly IToolsServices toolsServices;
        private readonly IPostContentServices postContentServices;
        private readonly IMediaProjectionServices mediaProjectionServices;
        private readonly IPostServices postServices;
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        public CreateProject(
            ICategoryServices _categoryServices,
            IToolsServices _toolsServices,
            IPostContentServices _postContentServices,
            IMediaProjectionServices _mediaProjectionServices,
            IPostServices _postServices
        )
        {
            this.categoryServices = _categoryServices;
            this.toolsServices = _toolsServices;
            this.postContentServices = _postContentServices;
            this.mediaProjectionServices = _mediaProjectionServices;
            this.postServices = _postServices;
        }
        public async Task ExecuteAsync(PostRequest postRequest)
        {
            // ValidatePostRequest(postRequest);
            // var tools = DeserializeTools(postRequest.Tools);
            // var categories = DeserializeCategories(postRequest.Categories);
            // var postContents = DeserializePostContents(postRequest.PostContents);
            // var post = new Post(postRequest.Status);
            // var mediasToCommit = new List<MediaFile>();
            // var mediasToDelete = new List<MediaFile>();
            // await ProcessPostContents(post, postContents, mediasToCommit, mediasToDelete);
            // await ProcessCategories(post, categories);
            // await ProcessTools(post, tools);
            // var media = await mediaFileServices.SaveImageAsync(postRequest.ImgFile!, "media/posts");
            // post.AddImgUrl(media!.Path);
            // mediasToCommit.Add(media);
            // await this.postServices.Save(post);
            // await CommitMedias(mediasToCommit);
            // await DeleteMedias(mediasToDelete);
        }
        private static void ValidatePostRequest(PostRequest postRequest)
        {
            var validationError = ValidationHelper.Validate(postRequest);
            if (validationError.Count > 0)
            {
                var errors = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {errors}");
            }
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
        private async Task ProcessCategories(Post post, List<CategoryDeserialize> categoriesRequest)
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
        private async Task ProcessTools(Post post, List<ToolDeserialize> toolRequests)
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
        // private async Task ProcessPostContents(Post post, List<PostContentDeserialize> postContents, List<MediaFile> mediasToCommit, List<MediaFile> mediasToDelete)
        // {
        //     foreach (var item in postContents)
        //     {
        //         var validationError = ValidationHelper.Validate(item);
        //         if (validationError.Count > 0)
        //         {
        //             var errors = string.Join(", ", validationError.Select(e => e.ErrorMessage));
        //             throw new ValidationException($"Erro ao validar dados: {errors}");
        //         }
        //         var postContent = await this.postContentServices.FindBy(pc => pc.Slug == item.Slug && pc.LanguageId == item.LanguageId);
        //         if (postContent != null)
        //             throw new ValidationException("Erro ao validar dados");
        //         postContent = new PostContent(post.Id, item.LanguageId, item.Title, item.Description, item.Content, item.Slug);
        //         var toRemoveImages = item.ImagesUrls.Where(image => !item.Content.Contains(image)).ToList();
        //         foreach (var removeImageUrl in toRemoveImages)
        //         {
        //             var mediaFileContent = await this.mediaFileServices.GetByPath(removeImageUrl);
        //             if (mediaFileContent != null)
        //                 if (!mediasToDelete.Any(m => m.Id == mediaFileContent.Id))
        //                     mediasToDelete.Add(mediaFileContent);
        //             item.ImagesUrls.Remove(removeImageUrl);
        //         }
        //         foreach (var addImageUrl in item.ImagesUrls)
        //         {
        //             var mediaFileContent = await this.mediaFileServices.GetByPath(addImageUrl);
        //             if (mediaFileContent != null)
        //             {
        //                 if (!mediasToCommit.Any(m => m.Id == mediaFileContent.Id))
        //                     mediasToCommit.Add(mediaFileContent);
        //             }
        //         }
        //         post.AddPostContent(postContent);
        //     }
        // }
        // private async Task CommitMedias(List<MediaFile> mediasToCommit)
        // {
        //     foreach (var media in mediasToCommit)
        //     {
        //         media.Commit();
        //         await this.mediaFileServices.Update(media);
        //     }
        // }
        // private async Task DeleteMedias(List<MediaFile> mediasToDelete)
        // {
        //     foreach (var mediaDelete in mediasToDelete)
        //     {
        //         await this.mediaFileServices.DeleteImageAsync(mediaDelete);
        //     }
        // }
    }
}