using PostService.Application.DTOs.Request;

namespace PostService.Application.UseCases.LinkTypes.Interfaces
{
    public interface IUpdateLinkType
    {
        Task ExecuteAsync(Guid Id, LinkTypeRequest request);
    }
}