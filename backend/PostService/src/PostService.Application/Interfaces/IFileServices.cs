using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PostService.Application.Interfaces
{
    public interface IFileServices
    {
        Task<string?> SaveImageAsync(IFormFile file, string folder);
        void DeleteImage(string imgUrl);
    }
}