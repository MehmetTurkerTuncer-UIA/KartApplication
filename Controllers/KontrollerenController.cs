using Microsoft.AspNetCore.Mvc;
using KartApplication.Models;
using System;
using System.Linq;
using KartApplication.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace KartApplication.Controllers
{
   // [Authorize(Roles = UserRoles.Role_Kontrolleren)]

    public class KontrollerenController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public KontrollerenController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Giriş yapan kullanıcının ID'sini al
            var currentUser = await _userManager.GetUserAsync(User);
            var currentUserId = currentUser?.Id;

            // Eğer kullanıcı oturum açmamışsa giriş sayfasına yönlendir
            if (currentUserId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Giriş yapan kullanıcıya atanmış Sak kayıtlarını filtrele
            var assignedSaks = _context.SakModels
                .Where(s => s.AssignedKontrollerenId == currentUserId)
                .ToList();

            return View(assignedSaks); // Filtrelenmiş Sak listesi Index view'ına gönderilir
        }
        public IActionResult Detaljer()
        {
            return View();

        }


    }
}