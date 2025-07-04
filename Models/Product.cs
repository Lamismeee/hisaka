using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HISAKA.Web.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        [StringLength(255)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mô tả sản phẩm là bắt buộc.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm là bắt buộc.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0.")]
        public decimal Price { get; set; }

        public decimal? DiscountedPrice { get; set; } // Giá khuyến mãi, có thể null

        [StringLength(500)]
        public string? ImageUrl { get; set; } // Đường dẫn ảnh sản phẩm

        [StringLength(100)]
        public string? Brand { get; set; } // Thương hiệu

        [Display(Name = "Category")]
        public int CategoryId { get; set; } // Khóa ngoại tới Category

        // Navigation property
        public Category? Category { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<ShoppingCartItem>? ShoppingCartItems { get; set; }

        // Các thuộc tính khác theo yêu cầu (loại da, công dụng...) có thể được thêm dưới dạng chuỗi hoặc một bảng riêng nếu phức tạp
        [StringLength(255)]
        public string? SkinType { get; set; } // Loại da phù hợp

        [StringLength(255)]
        public string? Benefits { get; set; } // Công dụng chính

        public double AverageRating { get; set; } = 0; // Điểm đánh giá trung bình
        public int ReviewCount { get; set; } = 0; // Tổng số lượt đánh giá
    }
}