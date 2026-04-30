using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PostService.Application.Interfaces;

namespace PostService.Application.Services
{
    public class FileServices : IFileServices
    {
        private readonly IWebHostEnvironment environment;
        public FileServices(IWebHostEnvironment _environment)
        {
            this.environment = _environment;
        }

        public void DeleteImage(string imgUrl)
        {
            var pathImage = Path.Combine(
                this.environment.WebRootPath,
                imgUrl
            );
            if(!File.Exists(pathImage))
                throw new Exception("Arquivo não encontrado");
            File.Delete(pathImage);
        }

        public async Task<string?> SaveImageAsync(IFormFile file, string folder)
        {
            if(file == null || file.Length == 0)
                return null;
            var uploadsFolder = Path.Combine(
                environment.WebRootPath,
                folder
            );
            if(!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}"; 
            var filePath = Path.Combine(uploadsFolder, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            return $"{folder}/{fileName}";
        }
    }
}