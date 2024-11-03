using Microsoft.AspNetCore.Mvc;
using KartApplication.Models;
using System;
using System.Linq;
using KartApplication.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

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
            var assignedSaks = await _context.SakModels
.Where(s => s.AssignedKontrollerenId == currentUserId)
.ToListAsync();

           
            return View(assignedSaks); // Filtrelenmiş Sak listesi Index view'ına gönderilir
        }
        public async Task<IActionResult> Detaljer(int id)
        {
            // İlgili Sak kaydını veritabanından getir
            var sak = await _context.SakModels.FindAsync(id);

            // Eğer Sak bulunamazsa veya atanmış Kontrolleren kullanıcısı giriş yapan kullanıcı değilse 404 döndür
            var currentUser = await _userManager.GetUserAsync(User);
            if (sak == null || sak.AssignedKontrollerenId != currentUser?.Id)
            {
                return NotFound();
            }

            return View(sak); // Sak kaydı Detaljer görünümüne gönderilir
        }

        // Status ve Kontroll Status Güncelleme İşlemi
        [HttpPost]
        public IActionResult UpdateStatus(int id, string KontrollerenDescription, string kontrolStatus)
        {
            Console.WriteLine("id değeri: " + id);
            SakModel? sakModel = _context.SakModels.FirstOrDefault(s => s.Id == id);

            if (sakModel == null)
            {
                return NotFound();
            }

            // SakStatus güncelleme
            if (Enum.TryParse(kontrolStatus, out KontrolStatus newKontrolStatus))
            {
                
                sakModel.KontrolStatus = newKontrolStatus;
            }

            // Yeni yorumu güncelle
            sakModel.KontrollerenDescription = KontrollerenDescription;


            _context.SaveChanges(); // Değişiklikleri kaydet
            return RedirectToAction("Detaljer", new { id }); // Güncelleme sonrası Detaljer sayfasına dön
        }



    }
}