using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer

    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
                RoleManager<IdentityRole> RoleManager,
                ApplicationDbContext db)
        {
            _roleManager = RoleManager;
                _userManager = userManager;
                _db = db;
        }
        public void Initialize()
        {
            // di chuyển nếu chúng không được áp dụng

            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }

            }
            catch (Exception ex)
            {

            }

            //tạo vai trò nếu chúng không được tạo

            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();


                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@tien.com",
                    Email = "diuthanh88@gmail.com",
                    Name = "Diu Thanh",
                    PhoneNumber = "1112345678",
                    StreetAddress = "Tess 123",
                    Status = "IL",
                    PostalCode = "2345",
                    City = "96 dinh cong"
                }, "Diuthanh88.").GetAwaiter().GetResult();

                ApplicationUser user = _db.applicationUsers.FirstOrDefault(u => u.Email == "diuthanh88@gmail.com");
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }

            // nếu vai trò không được tạo, thì chúng tôi cũng sẽ tạo người dùng quản trị
            return;

        }
    }
}
