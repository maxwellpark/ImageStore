using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;

namespace ImageStore.Controllers.MvcControllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public HomeController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public IActionResult Index()
        {
            var dirPath = Path.Combine(_environment.ContentRootPath, "Images");
            var files = Directory.GetFiles(dirPath, "*.jpeg", SearchOption.AllDirectories);
            var fileNames = files.Select(file => Path.GetFileName(file));
            return View(fileNames);
        }
    }
}
