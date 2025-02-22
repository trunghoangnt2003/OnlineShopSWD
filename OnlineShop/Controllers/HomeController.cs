using Microsoft.AspNetCore.Mvc;
using OnlineShop.Interfaces;
using OnlineShop.Models;
using System.Diagnostics;

namespace OnlineShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPhotoService _photoService;
        public HomeController(ILogger<HomeController> logger, IPhotoService photoService)
        {
            _logger = logger;
            _photoService = photoService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var result = await _photoService.AddPhotoAsync(imageFile);
                ViewBag.Message = "Ảnh đã tải lên thành công!";
                ViewBag.FileName = result.Url.ToString();
                return View("Index");
            }

            ViewBag.Message = "Vui lòng chọn một file!";
            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
