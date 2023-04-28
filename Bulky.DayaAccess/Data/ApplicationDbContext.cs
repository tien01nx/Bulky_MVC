using Bulky.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAcess.Data
{

    // kế thừa từ DbContext
    // DbContext đại diện cho một phiên làm việc với cơ sở dữ liệu
    public class ApplicationDbContext : DbContext
    {
        // Tạo và kết nối cơ sở dữ liệu 
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base (options)
        {
            
           
        }

        //prop 
        //đại diện cho một phiên làm việc với cơ sở dữ liệu
        //Categories sẽ giúp bạn truy xuất và thao tác(thêm, xóa, sửa) với dữ liệu trong bảng Category

        public DbSet <Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            modelBuilder.Entity<Category>().HasData(new
                Category
            { Id = 1, Name = "Action", DisplayOrder = 1 },

                new
                Category
                { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                new
                Category
                { Id = 3, Name = "History", DisplayOrder = 3 });
        }
    }
}
