using PostService.Application.DTOs.Request;
using PostService.Application.UseCases.Projects.Interfaces;

namespace PostService.Application.UseCases.Projects
{
    public class CreateProject : ICreateProject
    {
        public Task ExecuteAsync(PostRequest postRequest)
        {
            ValidatePostRequest(postRequest);
            throw new NotImplementedException();
        }
        private void ValidatePostRequest(PostRequest postRequest)
        {
            
        }
        private async void DeserializeTools(string Tools)
        {
            
        }
    }
}