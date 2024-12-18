using KartApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using Microsoft.AspNetCore.Identity;
using KartApplication.Models;
using KartApplication.Utilities;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();   //.NET Core ve MVC'de CSRF koruması, Anti-Forgery Middleware ile sağlanır.Bu çağrı, anti-forgery middleware'ı otomatik olarak ekler.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(10, 5, 11))));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>()
                                    .AddDefaultTokenProviders(); // <IdentityUser> i  <ApplicationUser> olarak degistirdim.

builder.Services.AddRazorPages();                //.NET Core ve MVC'de CSRF koruması, Anti-Forgery Middleware ile sağlanır.Bu çağrı, anti-forgery middleware'ı otomatik olarak ekler.


builder.Services.AddDistributedMemoryCache();  // Session için geçici bir bellek cache’i kullanılır
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Session süresi (örneğin 30 dakika)
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;  // GDPR ve diğer kurallar için gerekli
});


builder.Services.AddScoped<IEmailSender, EmailSender>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    // Güvenlik başlıkları
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("Referrer-Policy", "no-referrer");

    // Bir sonraki middleware'e geçiş
    await next.Invoke();
});


// Pipeline yapılandırması
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();  // Bunu ekledim SignInManager ve UserManager gibi kimlik doğrulama hizmetlerinin çalışabilmesi için 
app.UseAuthorization();
app.MapRazorPages();
app.UseSession();   // Session middleware'i ekleyin

app.MapGet("/", async context =>
{
    context.Response.Redirect("/Identity/Account/Login");
    await context.Response.CompleteAsync(); // Bu satır ile yanıt tamamlanır.
});


app.MapControllerRoute(
    name: "default",
   pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();