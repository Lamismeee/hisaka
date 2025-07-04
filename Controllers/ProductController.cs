using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HISAKA.Web.Data;
using HISAKA.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HISAKA.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ApplicationDbContext context, ILogger<ProductController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Action cho Danh sách sản phẩm (RQ004)
        // Hiển thị tất cả sản phẩm hoặc sản phẩm theo danh mục (category)
        // Có thể thêm các tham số lọc, sắp xếp sau này
        public async Task<IActionResult> List(int? categoryId, string? searchTerm)
        {
            IQueryable<Product> products = _context.Products
                                                .Include(p => p.Category) // Kéo theo thông tin danh mục
                                                .Include(p => p.Reviews); // Kéo theo đánh giá để tính AverageRating

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
                ViewData["CurrentCategory"] = await _context.Categories.FindAsync(categoryId.Value);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm));
                ViewData["SearchTerm"] = searchTerm;
            }
            
            // Tính toán AverageRating và ReviewCount
            var productList = await products.ToListAsync();
            foreach (var product in productList)
            {
                if (product.Reviews != null && product.Reviews.Any())
                {
                    product.AverageRating = product.Reviews.Average(r => r.Rating);
                    product.ReviewCount = product.Reviews.Count();
                }
            }

            ViewBag.Categories = await _context.Categories.ToListAsync(); // Để hiển thị bộ lọc danh mục

            return View(productList);
        }

        // Action cho Chi tiết sản phẩm (RQ005)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                                        .Include(p => p.Category)
                                        .Include(p => p.Reviews)
                                            .ThenInclude(r => r.User) // Kéo theo thông tin người đánh giá
                                        .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            
            // Tính toán AverageRating và ReviewCount nếu chưa có
            if (product.Reviews != null && product.Reviews.Any())
            {
                product.AverageRating = product.Reviews.Average(r => r.Rating);
                product.ReviewCount = product.Reviews.Count();
            }

            return View(product);
        }
    }
}