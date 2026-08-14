using PostService.Application.DTOs.Request;

namespace PostService.Application.UseCases.LinkTypes.Interfaces
{
    public interface IDeleteLinkType
    {
        Task ExecuteAsync(Guid Id);
    }
}