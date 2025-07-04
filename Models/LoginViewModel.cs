using System.ComponentModel.DataAnnotations;

namespace HISAKA.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email hoặc số điện thoại là bắt buộc.")]
        [Display(Name = "Email/Số điện thoại")]
        public string EmailOrPhoneNumber { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Ghi nhớ tôi?")]
        public bool RememberMe { get; set; }
    }
}