using Azure.Identity; // Eğer kullanıyorsanız
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KartApplication.Models; 
using System.Threading.Tasks; // Async metotlar için

namespace KartApplication.Areas.Identity.Controllers // Identity alanındaki namespace
{
    [Area("Identity")] // Area attribute'u
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager; // UserManager'ı tanımlayın
        }

        public IActionResult Users()
        {
            return View();
        }

        public IActionResult VerifyEmail()
        {
            return View();
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account", new { area = "Identity" }); // Çıkış yaptıktan sonra Login sayfasına yönlendir
        }
    }
}
