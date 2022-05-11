using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ImageStore.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly ILogger<ImageController> _logger;
        private readonly IWebHostEnvironment _environment;

        public ImageController(ILogger<ImageController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        /// <summary>
        /// Receives form-data Content-Type with image data and saves it to ~/images/
        /// </summary>
        /// <param name="image"></param>
        [HttpPost]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, ValueCountLimit = int.MaxValue)]
        public async Task<IActionResult> Post(IFormFile image)
        {
            _logger.LogInformation($"{DateTime.UtcNow},Image POST received.");

            var contextFiles = HttpContext.Request.Form.Files;
            var message = string.Empty;

            try
            {
                foreach (var file in HttpContext.Request.Form.Files)
                {
                    if (file == null || file.Length <= 0)
                    {
                        _logger.LogWarning($"File {file.FileName} has no content");
                        return new BadRequestObjectResult(message);
                    }

                    var filePath = Path.Combine(_environment.ContentRootPath, "Images", file.FileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    message = "Image uploaded: " + file.FileName;
                    _logger.LogInformation(message);
                }

                message = "Upload successful";
                _logger.LogInformation(message);
                return Ok(message);
            }
            catch (Exception ex)
            {
                message = "An error occurred";
                _logger.LogError(ex, message);
                return new BadRequestObjectResult(message);
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation($"{DateTime.UtcNow},GET received for image data");
            var nameParam = HttpContext.Request.Query["name"];

            if (string.IsNullOrWhiteSpace(nameParam))
            {
                return new BadRequestObjectResult("Name param missing from query string.");
            }
            var imageName = nameParam.ToString();

            try
            {
                var bytes = System.IO.File.ReadAllBytes(Path.Combine(_environment.ContentRootPath, "Images", imageName + ".jpeg"));
                return File(bytes, "image/jpeg");
            }
            catch (Exception ex)
            {
                var message = "An error occurred";
                _logger.LogError(ex, message);
                return new BadRequestObjectResult(message);
            }
        }
    }
}
