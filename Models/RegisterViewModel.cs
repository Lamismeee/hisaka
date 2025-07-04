using System.ComponentModel.DataAnnotations;

namespace HISAKA.Web.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email hoặc số điện thoại là bắt buộc.")]
        [Display(Name = "Email/Số điện thoại")]
        [StringLength(255)]
        // Có thể thêm validation email/phone regex nếu cần
        public string EmailOrPhoneNumber { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [StringLength(100, ErrorMessage = "{0} phải có ít nhất {2} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }

        [StringLength(100)]
        [Display(Name = "Họ và tên")]
        public string? FullName { get; set; } // Không bắt buộc khi đăng ký ban đầu
    }
}