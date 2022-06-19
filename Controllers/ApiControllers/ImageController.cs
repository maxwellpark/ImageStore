using ImageStore.Data.Models;
using ImageStore.Data.Repositories;
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
        private readonly IImageRepository _imageRepository;
        private readonly IWebHostEnvironment _environment;

        public ImageController(ILogger<ImageController> logger, IImageRepository imageRepository, IWebHostEnvironment environment)
        {
            _logger = logger;
            _imageRepository = imageRepository ?? throw new ArgumentNullException(nameof(imageRepository));
            _environment = environment;
        }

        /// <summary>
        /// Receives form-data Content-Type with image data and saves it to ~/images/
        /// </summary>
        /// <param name="image"></param>
        [HttpPost]
        [Consumes("multipart/form-data")]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, ValueCountLimit = int.MaxValue)]
        public async Task<IActionResult> UploadImage(IFormCollection formData)
        {
            _logger.LogInformation("Image POST received.");
            var message = string.Empty;

            try
            {
                // Image metadata from body 
                var imageName = formData["name"];
                var imageCaption = formData["caption"];
                var creationDate = DateTime.Now.ToString("dd/MM/yyyy");
                var tags = formData["tags"];

                foreach (var file in HttpContext.Request.Form.Files)
                {
                    if (file == null || file.Length <= 0)
                    {
                        _logger.LogWarning(string.Format("File with name '{0}' has no content", file.FileName));
                        return new BadRequestObjectResult(new ImageUploadResult("failure", message));
                    }

                    var filePath = Path.Combine(_environment.ContentRootPath, "Images", file.FileName);
                    _logger.LogInformation("New file path: " + filePath);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    var image = new Image(imageName, filePath, imageCaption, creationDate, tags);

                    try
                    {
                        await _imageRepository.AddImageAsync(image);
                    }
                    catch (Exception)
                    {
                        // Rollback IO operation if exception thrown in EF 
                        _logger.LogError("Deleting file because an error occurred when writing to the database");
                        System.IO.File.Delete(filePath);
                        throw;
                    }

                    message = "Image uploaded: " + file.FileName;
                    _logger.LogInformation(message);
                }

                message = "Upload successful";
                _logger.LogInformation(message);
                return Ok(new ImageUploadResult("success", message, creationDate));
            }
            catch (Exception ex)
            {
                message = "An unexpected error occurred";
                _logger.LogError(ex, message);
                return new BadRequestObjectResult(new ImageUploadResult("failure", message));
            }
        }

        [HttpGet]
        public IActionResult GetImage()
        {
            _logger.LogInformation("GET received for image data");
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
                var message = "An unexpected error occurred";
                _logger.LogError(ex, message);
                return new BadRequestObjectResult(message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetImage(int id)
        {
            _logger.LogInformation("GET received for image by ID " + id);

            try
            {
                var image = _imageRepository.GetImageById(id);
                return new OkObjectResult(image);
            }
            catch (Exception ex)
            {
                var message = "An unexpected error occurred";
                _logger.LogError(ex, message);
                return new BadRequestObjectResult(message);
            }
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> UpdateCaption(int id)
        {
            _logger.LogInformation("PATCH received for image data by ID " + id);
            var captionParam = HttpContext.Request.Query["caption"];

            if (string.IsNullOrWhiteSpace(captionParam))
            {
                return new BadRequestObjectResult("Caption param missing from query string.");
            }

            try
            {
                if (await _imageRepository.UpdateCaptionAsync(id, captionParam))
                {
                    return new OkObjectResult("Caption updated successfully");
                }
                return new BadRequestObjectResult("Failed to update caption");
            }
            catch (Exception ex)
            {
                var message = "An unexpected error occurred";
                _logger.LogError(ex, message);
                return new BadRequestObjectResult(message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            _logger.LogInformation("DELETE received for image data by ID " + id);

            try
            {
                if (await _imageRepository.DeleteImageByIdAsync(id))
                {
                    return new OkObjectResult("Image deleted updated successfully");
                }
                return new BadRequestObjectResult("Failed to delete image");
            }
            catch (Exception ex)
            {
                var message = "An unexpected error occurred";
                _logger.LogError(ex, message);
                return new BadRequestObjectResult(message);
            }
        }
    }
}
