using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO; // Đảm bảo có using System.IO;

namespace HISAKA.Web.Data
{
    // Đảm bảo namespace này trùng với namespace của ApplicationDbContext
    // Hoặc bạn có thể tạo một namespace mới và thêm using vào ApplicationDbContextFactory
    // Ví dụ: namespace HISAKA.Web.Design
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // CÁCH MỘT (Phổ biến nhất): Sử dụng Directory.GetCurrentDirectory() nếu bạn chạy lệnh từ thư mục HISAKA.Web
            var basePath = Directory.GetCurrentDirectory();

            // CÁCH HAI (Nếu bạn chạy lệnh từ thư mục gốc của Solution HISAKA/):
            // var basePath = Path.Combine(Directory.GetCurrentDirectory(), "HISAKA.Web");

            // Để Debug: In ra đường dẫn cơ sở đang được sử dụng
            // Console.WriteLine($"Base Path for Configuration: {basePath}");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Bắt buộc phải có
                .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true) // Có thể có hoặc không
                .Build();

            // Để Debug: In ra chuỗi kết nối đã đọc được
            // var connectionStringDebug = configuration.GetConnectionString("DefaultConnection");
            // Console.WriteLine($"Connection String Read: {connectionStringDebug}");

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found in appsettings.json or appsettings.Development.json. " +
                                                    "Please ensure appsettings.json is in the correct directory and contains 'DefaultConnection'.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}