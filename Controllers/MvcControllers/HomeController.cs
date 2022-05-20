using ImageStore.Data.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ImageStore.Controllers.MvcControllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IMemoryCache _memoryCache;
        private readonly IImageRepository _imageRepository;
        private readonly TimeSpan _cacheExpirationTime = TimeSpan.FromMinutes(10);

        private readonly string _imageFilePathsKey = "imageFilePaths";
        private readonly string _imageDirectory;
        private readonly string[] _includePatterns = new[] { "*.jpg", "*.jpeg" };

        public HomeController(IWebHostEnvironment environment, IMemoryCache memoryCache, IImageRepository imageRepository)
        {
            _environment = environment;
            _memoryCache = memoryCache;
            _imageRepository = imageRepository ?? throw new ArgumentNullException(nameof(imageRepository));
            _imageDirectory = Path.Combine(_environment.ContentRootPath, "Images");
        }

        public IActionResult Index()
        {
            var images = _imageRepository.GetImages();
            return View(images);
        }

        private List<string> GetImageFilePaths()
        {
            var filePaths = _memoryCache.Get<List<string>>(_imageFilePathsKey);

            if (filePaths != null)
            {
                return filePaths;
            }
            var matcher = new Matcher();
            matcher.AddIncludePatterns(_includePatterns);

            var dirInfoWrapper = new DirectoryInfoWrapper(new DirectoryInfo(_imageDirectory));
            var matchingResult = matcher.Execute(dirInfoWrapper);

            if (matchingResult == null)
            {
                return new List<string>();
            }
            filePaths = matchingResult.Files.Select(file => file.Path)?.ToList();
            _memoryCache.Set(_imageFilePathsKey, filePaths, _cacheExpirationTime);
            return filePaths;
        }
    }
}
