using Microsoft.AspNetCore.Mvc;
using OnlineShop.Interfaces;
using OnlineShop.Models;
using OnlineShop.Services;
using System.Diagnostics;

namespace OnlineShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPhotoService _photoService;
        private readonly IVnPayService _vnPayService;
        private readonly IOrderShippingService _orderShippingService;
        public HomeController(ILogger<HomeController> logger, IPhotoService photoService, IVnPayService vnPayService ,IOrderShippingService orderShippingService)
        {
            _logger = logger;
            _photoService = photoService;
            _vnPayService = vnPayService;
            _orderShippingService = orderShippingService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            VnPaymentRequestModel vnPaymentRequestModel = new VnPaymentRequestModel
            {
                Amount = 10000,
                CreatedDate = DateTime.Now,
                Description = "Thanh toan don hang",
                FullName = "Trung",
                OrderId = new Random().Next(10, 100),
            };

            return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPaymentRequestModel)); ;
        }
        public IActionResult CreateOrder()
        {
            VnPaymentRequestModel vnPaymentRequestModel = new VnPaymentRequestModel
            {
                Amount = 10000,
                CreatedDate = DateTime.Now,
                Description = "Thanh toan don hang",
                FullName = "Trung",
                OrderId = new Random().Next(10, 100),
            };

            return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPaymentRequestModel)); ;
        }
        public IActionResult PaymentCallBack()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                return Json("PaymentFail");
            }
            return Json("PaymentSuccess");
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrderGHN()
        {
            var model = new ShippingOrderModel
            {
                PaymentTypeId = 2,
                Note = "Tintest 123",
                RequiredNote = "KHONGCHOXEMHANG",
                FromName = "TinTest124",
                FromPhone = "0987654321",
                FromAddress = "72 Thành Thái, Phường 14, Quận 10, Hồ Chí Minh, Vietnam",
                FromWardName = "Phường 14",
                FromDistrictName = "Quận 10",
                FromProvinceName = "HCM",
                ReturnPhone = "0332190444",
                ReturnAddress = "39 NTT",
                ReturnDistrictId = null,
                ReturnWardCode = "",
                ClientOrderCode = "",
                ToName = "TinTest124",
                ToPhone = "0987654321",
                ToAddress = "72 Thành Thái, Phường 14, Quận 10, Hồ Chí Minh, Vietnam",
                ToWardCode = "20308",
                ToDistrictId = 1444,
                CodAmount = 200000,
                Content = "Theo New York Times",
                Weight = 200,
                Length = 1,
                Width = 19,
                Height = 10,
                PickStationId = 1444,
                DeliverStationId = null,
                InsuranceValue = 4000000,
                ServiceId = 0,
                ServiceTypeId = 2,
                Coupon = null,
                PickShift = new List<int> { 2 },
                Items = new List<Item>
                {
                    new Item
                    {
                        Name = "Áo Polo",
                        Code = "Polo123",
                        Quantity = 1,
                        Price = 200000,
                        Length = 12,
                        Width = 12,
                        Height = 12,
                        Weight = 1200,
                        Category = new Category
                        {
                            Level = "Áo"
                        }
                    }
                }
            };
            var respone = await _orderShippingService.CreateOrderShippingAsync(model);
            return Content(respone, "application/json");
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
