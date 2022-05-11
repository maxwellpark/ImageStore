using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageStore.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RemoteImageController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public RemoteImageController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, ValueCountLimit = int.MaxValue)]
        public async Task<IActionResult> Post()
        {
            try
            {
                var file = HttpContext.Request.Form.Files.FirstOrDefault();

                if (file == null || file.Length <= 0)
                {
                    return new BadRequestObjectResult($"File {file.FileName} has no content");
                }

                var filePath = Path.Combine(_environment.ContentRootPath, "Images", "Remote", file.FileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                return Ok("Image uploaded: " + file.FileName);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("An error occurred: " + ex.Message);
            }
        }
    }
}
