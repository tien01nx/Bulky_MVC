
//đối tượng này dùng để cấu hình và xây dựng ứng dụng.
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess;
using Bulky.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Bulky.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Điều này cho phép ứng dụng sử dụng Controllers và Views trong kiến trúc MVC.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));



// add quyen nguoi dung 
builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});
builder.Services.AddRazorPages();


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IEmailSender, EmailSender>();
//để xây dựng ứng dụng và lưu vào biến app.
var app = builder.Build();

// Nếu không phải môi trường phát triển
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//Sử dụng chuyển hướng HTTPS:
app.UseHttpsRedirection();
//Sử dụng tệp tĩnh:
//Cho phép ứng dụng phục vụ các tệp tĩnh, như CSS, JavaScript, hình ảnh, vv.
app.UseStaticFiles();

//Kích hoạt hệ thống định tuyến, cho phép ánh xạ các yêu cầu đến Controllers và Actions.
app.UseRouting();

app.UseAuthentication();
//Kích hoạt hệ thống ủy quyền, cho phép bảo vệ các tài nguyên dựa trên chính sách và đường dẫn.
app.UseAuthorization();

app.MapRazorPages();

/*app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");*/


app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");


app.Run();
