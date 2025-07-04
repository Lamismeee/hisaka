using Microsoft.AspNetCore.Mvc;
using HISAKA.Web.Models;
using HISAKA.Web.Data; // Cần thiết để tương tác với DbContext
using System.Security.Cryptography; // Để mã hóa mật khẩu
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Để sử dụng các phương thức của EF Core

namespace HISAKA.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ApplicationDbContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // --- ĐĂNG NHẬP (RQ002) ---
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Chống tấn công CSRF
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // Tìm người dùng theo Email hoặc Số điện thoại
                var user = await _context.Users
                                         .FirstOrDefaultAsync(u => u.EmailOrPhoneNumber == model.EmailOrPhoneNumber);

                if (user != null)
                {
                    // So sánh mật khẩu đã hash
                    if (VerifyPasswordHash(model.Password, user.PasswordHash))
                    {
                        // TODO: Implement Logic đăng nhập (ví dụ: HttpContext.SignInAsync - ASP.NET Core Identity)
                        // Hiện tại, chỉ thông báo thành công
                        _logger.LogInformation($"User {user.EmailOrPhoneNumber} logged in successfully.");
                        TempData["SuccessMessage"] = "Đăng nhập thành công!";

                        // Điều hướng sau khi đăng nhập thành công
                        // Theo yêu cầu: Điều hướng đến giỏ hàng hoặc lịch sử đơn hàng (hiện tại tạm về trang chủ)
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction("Index", "Home"); // Về trang chủ tạm thời
                    }
                }
                ModelState.AddModelError(string.Empty, "Email/Số điện thoại hoặc mật khẩu không đúng.");
            }
            return View(model);
        }

        // --- ĐĂNG KÝ (RQ003) ---
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // Kiểm tra xem Email/SĐT đã tồn tại chưa
                if (await _context.Users.AnyAsync(u => u.EmailOrPhoneNumber == model.EmailOrPhoneNumber))
                {
                    ModelState.AddModelError(nameof(model.EmailOrPhoneNumber), "Email hoặc số điện thoại này đã được sử dụng.");
                    return View(model);
                }

                // Tạo đối tượng User mới
                var newUser = new User
                {
                    EmailOrPhoneNumber = model.EmailOrPhoneNumber,
                    PasswordHash = HashPassword(model.Password), // Mã hóa mật khẩu
                    FullName = model.FullName,
                    RewardPoints = 0 // Mặc định 0 điểm thưởng
                };

                _context.Add(newUser);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"User {newUser.EmailOrPhoneNumber} registered successfully.");
                TempData["SuccessMessage"] = "Đăng ký tài khoản thành công! Bạn có thể đăng nhập ngay bây giờ.";

                return RedirectToAction(nameof(Login)); // Chuyển hướng về trang đăng nhập
            }
            return View(model);
        }

        // --- Hỗ trợ mã hóa mật khẩu ---
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            var hashedInput = HashPassword(password);
            return hashedInput == storedHash;
        }

        // Action Đăng xuất (Tạm thời)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            // TODO: Logic đăng xuất thực tế (ví dụ: HttpContext.SignOutAsync)
            TempData["SuccessMessage"] = "Bạn đã đăng xuất thành công.";
            return RedirectToAction("Index", "Home");
        }
    }
}