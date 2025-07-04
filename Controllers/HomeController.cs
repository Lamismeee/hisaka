using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HISAKA.Web.Models; // Đảm bảo đã include namespace này cho ErrorViewModel

namespace HISAKA.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Action cho Trang chủ (RQ001)
        public IActionResult Index()
        {
            // Trong tương lai: Logic để lấy sản phẩm nổi bật, ưu đãi từ database
            // Hiện tại, chúng ta chỉ trả về View mặc định.
            return View(); // Dòng này rất quan trọng để trả về Views/Home/Index.cshtml
        }

        // Action cho trang Privacy (tùy chọn, thường có trong mẫu dự án)
        public IActionResult Privacy()
        {
            return View();
        }

        // Action cho trang Error (thường có trong mẫu dự án, hiển thị lỗi)
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}