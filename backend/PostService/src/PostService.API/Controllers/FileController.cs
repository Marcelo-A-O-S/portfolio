using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PostService.Application.Interfaces;
using PostService.Application.DTOs.Request;

namespace PostService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileServices fileServices;
        private readonly IMediaFileServices mediaFileServices;
        public FileController(
            IFileServices _fileServices,
            IMediaFileServices _mediaFileServices)
        {
            this.fileServices = _fileServices;
            this.mediaFileServices = _mediaFileServices;
        }
        [HttpPost("Upload/Markdown")]
        public async Task<IActionResult> Upload([FromForm]ImageMarkdownCreate imageMarkdown)
        {
            if (ModelState.IsValid)
            {
                var mediaContentMarkDown = await this.mediaFileServices.SaveImageAsync(imageMarkdown.file, "media/markdown");
                if(mediaContentMarkDown is null)
                    return BadRequest("Erro ao salvar imagem");
                return Ok(new { url =  mediaContentMarkDown.Path });
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
    }
}