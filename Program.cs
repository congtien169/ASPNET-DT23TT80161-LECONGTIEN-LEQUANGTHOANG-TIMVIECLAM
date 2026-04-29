using Microsoft.EntityFrameworkCore;                    // Bắt buộc cho UseSqlServer
using Microsoft.AspNetCore.Identity;                   // Cho Identity
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Cho AddEntityFrameworkStores
using JobPortalApp.Data;

var builder = WebApplication.CreateBuilder(args);

// === ĐĂNG KÝ DbContext với SQL Server ===
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// === ĐĂNG KÝ ASP.NET Core Identity ===
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Thêm các service khác (giữ nguyên phần cũ của bạn)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();   // <<< PHẢI CÓ
app.UseAuthorization();    // <<< PHẢI CÓ

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    try
//    {
//        await DataSeeder.SeedAsync(services);
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"❌ Lỗi Seed Data: {ex.Message}");
//    }
//}

// ====================== SEED DATA ======================
//Console.WriteLine("🌱 Đang seed dữ liệu giả...");
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    try
//    {
//        await DataSeeder.SeedAsync(services);
//        Console.WriteLine("✅ SEED DATA THÀNH CÔNG - Đã tạo đầy đủ dữ liệu giả!");
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"❌ Lỗi khi seed data: {ex.Message}");
//    }
//}
// ======================================================

app.Run();

app.Run();