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

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(10, 5, 11))));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                                    .AddEntityFrameworkStores<ApplicationDbContext>()
                                    .AddDefaultTokenProviders(); // Endret fra <IdentityUser> til <ApplicationUser>
builder.Services.AddRazorPages();

builder.Services.AddDistributedMemoryCache();  // Midlertidig minnebuffer brukes til sesjon (session)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Sesjonsvarighet (f.eks. 30 minutter)
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;  // Nødvendig for GDPR og andre regler
});

builder.Services.AddScoped<IEmailSender, EmailSender>();

var app = builder.Build();

// Konfigurasjon av pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();  // Lagt til for å støtte autentiseringstjenester som SignInManager og UserManager
app.UseAuthorization();
app.MapRazorPages();
app.UseSession();   // Legg til session-middleware

app.MapGet("/", async context =>
{
    context.Response.Redirect("/Identity/Account/Login");
    await context.Response.CompleteAsync(); // Denne linjen fullfører responsen.
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
