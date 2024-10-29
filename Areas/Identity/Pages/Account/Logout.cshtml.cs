using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using KartApplication.Models;

namespace KartApplication.Areas.Identity.Pages.Account
{
    public class Logout : PageModel // Burada LogoutModel yerine Logout kullanılmalıdır.
    {
        private readonly SignInManager<ApplicationUser> _signInManager; 
        private readonly ILogger<Logout> _logger; // Burada da Logout kullanılmalıdır.

        public Logout(SignInManager<ApplicationUser> signInManager, ILogger<Logout> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnPost(string? returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
        }
    }
}
