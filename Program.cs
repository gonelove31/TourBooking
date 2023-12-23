using BookingTour.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using static Album.Mail.SendMailService;
using  Album.Mail;
using System.Configuration;
using Microsoft.AspNetCore.Hosting;
using App.Menu;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using App.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using BookingTour.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var services = builder.Services;
services.AddScoped<IViewRenderService, ViewRenderService>();
// Trong ConfigureServices của Startup.cs
services.AddScoped<UserActionHistoryHelper>();

services.AddRazorPages();
services.AddDbContext<TourContext>(options =>
{
    string url = builder.Configuration.GetConnectionString("TourContext");
    options.UseSqlServer(url);
});

services.AddRazorPages();
services.AddHttpContextAccessor();
services.AddIdentity<AppUser, IdentityRole>()
     .AddRoles<IdentityRole>()
      .AddEntityFrameworkStores<TourContext>()
      .AddDefaultTokenProviders();

//Identity/Account/Login
// dang ky indentity , role : vai tro


services.Configure<IdentityOptions>(options => {
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
    options.SignIn.RequireConfirmedAccount = true; // cau hinh bat xac thuc email moi duoc dang ky
});



services.AddDistributedMemoryCache();           // Đăng ký dịch vụ lưu cache trong bộ nhớ (Session sẽ sử dụng nó)
services.AddSession(cfg => {                    // Đăng ký dịch vụ Session
    cfg.Cookie.Name = "appmvc";                 // Đặt tên Session - tên này sử dụng ở Browser (Cookie)
    cfg.IdleTimeout = new TimeSpan(0, 30, 0);    // Thời gian tồn tại của Session
});

services.AddOptions();
var mailsetting =builder.Configuration.GetSection("MailSettings");
services.Configure<MailSettings>(mailsetting);
services.AddSingleton<IEmailSender, SendMailService>();




services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
services.AddTransient<AdminSidebarService>();
services.AddAuthorization(options => {
    options.AddPolicy("ViewManageMenu", builder => {
        builder.RequireAuthenticatedUser();
        builder.RequireRole(RoleName.Administrator);
    });
});
services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/Account/Login"; // Đường dẫn đến trang đăng nhập
        options.LogoutPath = "/Admin/Account/Logout"; // Đường dẫn đến trang đăng xuất
        options.AccessDeniedPath = "/Admin/Account/AccessDenied"; // Đường dẫn đến trang truy cập bị từ chối
    });


// Configure the HTTP request pipeline.
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();
app.UseRouting();

app.UseAuthentication(); //xac thuc danh tinh

app.UseAuthorization(); // sau khi xac thuc , thi dc lam gi

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//doc thong tin 1 user , kiem tra 1 user co dang nhap khong 


app.MapRazorPages();
app.Run();
