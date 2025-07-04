using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HISAKA.Web.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Email hoặc số điện thoại là bắt buộc.")]
        [StringLength(255)]
        public string EmailOrPhoneNumber { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [StringLength(255)]
        public string PasswordHash { get; set; } // Sẽ lưu trữ mật khẩu đã hash

        [StringLength(100)]
        public string? FullName { get; set; } // Tên đầy đủ, có thể null ban đầu

        [StringLength(500)]
        public string? Address { get; set; } // Địa chỉ, có thể null ban đầu

        public int RewardPoints { get; set; } = 0; // Điểm thưởng, mặc định là 0

        // Navigation property
        public ICollection<ShoppingCartItem>? ShoppingCartItems { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        // public ICollection<Order>? Orders { get; set; } // Sẽ thêm khi phát triển tính năng Order
    }
}