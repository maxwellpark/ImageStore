using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ImageStore.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocalImageController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _environment;

        public LocalImageController(IHttpClientFactory factory, IWebHostEnvironment environment)
        {
            _httpClient = factory.CreateClient();
            _environment = environment;
        }

        [HttpGet]
        [Route("{fileName}")]
        public async Task<IActionResult> Get(string fileName)
        {
            try
            {
                var bytes = System.IO.File.ReadAllBytes(Path.Combine(_environment.ContentRootPath, "Images", "Local", fileName));

                var multipartFormData = new MultipartFormDataContent();
                multipartFormData.Add(new ByteArrayContent(bytes), "image_data", fileName);

                var uri = new Uri("https://localhost:44385/api/RemoteImage");

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = uri,
                    Content = multipartFormData
                };

                var response = await _httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                return Ok(content);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }
    }
}
