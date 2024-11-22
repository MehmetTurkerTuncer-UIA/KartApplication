using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KartApplication.Models;
using System;
using System.Threading.Tasks;

namespace KartApplication.Controllers
{
    public class GjestController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public GjestController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Kullanıcı oluştur ve Gjest Index'e yönlendir
        [HttpPost]
        public async Task<IActionResult> CreateGjest()
        {
            var uniqueId = Guid.NewGuid().ToString("N");
            var email = $"gjest_{uniqueId}@gjest.com";
            var username = $"Gjest_{uniqueId}";

            var user = new ApplicationUser
            {
                UserName = username,
                Email = email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Gjest");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Gjest");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("Error");
        }

        // Gjest Index sayfası
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // Gjest Beskrivelse sayfası
        [HttpPost]
        public IActionResult Beskrivelse(SakModel model)
        {
            if (ModelState.IsValid)
            {
                // Verileri işleme alabilir ve saklayabilirsiniz
                return RedirectToAction("Beskrivelse");
            }

            return View("Index", model);
        }
    }
}
