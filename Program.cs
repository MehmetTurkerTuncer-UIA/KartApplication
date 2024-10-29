using KartApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using Microsoft.AspNetCore.Identity;
using KartApplication.Models;
using KartApplication.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(10, 5, 11))));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>(); // <IdentityUser> i  <ApplicationUser> olarak degistirdim.
builder.Services.AddRazorPages();

//builder.Services.AddScoped<IEmailSender, EmailSender>();

var app = builder.Build();

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

app.MapGet("/", async context =>
{
    context.Response.Redirect("/Identity/Account/Login");
    await context.Response.CompleteAsync(); // Bu satır ile yanıt tamamlanır.
});


app.MapControllerRoute(
    name: "default",
   pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();