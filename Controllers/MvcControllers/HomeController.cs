using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ImageStore.Controllers.MvcControllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        private readonly string _imageDirectory;
        private readonly string[] _includePatterns = new[] { "*.jpg", "*.jpeg" };

        public HomeController(IWebHostEnvironment environment)
        {
            _environment = environment;
            _imageDirectory = Path.Combine(_environment.ContentRootPath, "Images");
        }

        public IActionResult Index()
        {
            var filePaths = GetImageFilePaths();
            return View(filePaths);
        }

        private IEnumerable<string> GetImageFilePaths()
        {
            var matcher = new Matcher();
            matcher.AddIncludePatterns(_includePatterns);

            var dirInfoWrapper = new DirectoryInfoWrapper(new DirectoryInfo(_imageDirectory));
            var matchingResult = matcher.Execute(dirInfoWrapper);

            var paths = matchingResult.Files.Select(file => file.Path);
            return paths;
        }
    }
}
