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
        public FileController(IFileServices _fileServices)
        {
            this.fileServices = _fileServices;
        }
        [HttpPost("Upload/Markdown")]
        public async Task<IActionResult> Upload([FromForm]ImageMarkdownCreate imageMarkdown)
        {
            if (ModelState.IsValid)
            {
                var url = await this.fileServices.SaveImageAsync(imageMarkdown.file, "media/markdown");
                return Ok(new { url });
            }
            var errors = ModelState.Values.Select(e => e.Errors);
            return BadRequest(errors);
        }
    }
}