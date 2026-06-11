using MediaService.Application.Exceptions;
using MediaService.Domain.Entities;
namespace MediaService.Application.Constants
{
    public static class FolderResolver
    {
        public static string Resolve(string ownerType)
        {
            return ownerType switch
            {
                OwnerTypes.Post => MediaFolders.Posts,
                OwnerTypes.Tool => MediaFolders.Tools,
                OwnerTypes.Certificate => MediaFolders.Certificates,

                _ => throw new ValidationException(
                    "Tipo de proprietário inválido."
                )
            };
        }
    }
}