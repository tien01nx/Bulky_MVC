
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
using Stripe;
using Azure.Core;
using Bulky.DataAccess.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Điều này cho phép ứng dụng sử dụng Controllers và Views trong kiến trúc MVC.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

// add quyen nguoi dung 
builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

builder.Services.AddAuthentication().AddFacebook(option =>
{
    option.AppId = "1664891260599422";
    option.AppSecret = "f824d99c013c31018f3d6c5f001c9486";
});


//  phiên đăng nhập được lưu trữ trong bộ nhớ
builder.Services.AddDistributedMemoryCache();
//Session trong ứng dụng. Session được sử dụng để lưu trữ dữ liệu trên toàn bộ các yêu cầu HTTP
//của một user, nó cho phép lưu trữ các thông tin người dùng
//như giỏ hàng, lịch sử truy cập, thông tin đăng nhập, v.v. trong khi người dùng đang duyệt web trang
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100); // thời gian hoạt động của sesion hết hạn là 100 giây
    options.Cookie.HttpOnly = true; //giới hạn truy cập của cookie trong HTTP requests
    options.Cookie.IsEssential = true; //xác định cookie là bắt buộc hay không để sử dụng cho Session
});
builder.Services.AddRazorPages();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();


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

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
//Kích hoạt hệ thống định tuyến, cho phép ánh xạ các yêu cầu đến Controllers và Actions.
app.UseRouting();

app.UseAuthentication();
//Kích hoạt hệ thống ủy quyền, cho phép bảo vệ các tài nguyên dựa trên chính sách và đường dẫn.
app.UseAuthorization();

app.UseSession();
SeedDatabase();

app.MapRazorPages();

/*app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");*/


app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");


app.Run();


void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInittializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInittializer.Initialize();
    }
}