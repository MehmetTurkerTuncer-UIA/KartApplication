using Microsoft.AspNetCore.Mvc;
using KartApplication.Models;
using KartApplication.Data;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using System.Security.Claims;
using System.Threading.Tasks;



namespace KartApplication.Controllers
{
    [Authorize(Roles = UserRoles.Role_Saksbehandler)]
    public class SaksbehandlerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SaksbehandlerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Saksbehandler giriş sayfası
        public IActionResult Index()
        {
            List<SakModel> objSakList = _context.SakModels
                .Where(s => s.Status == SakStatus.SakMottatt || s.Status == SakStatus.UnderBehandling)
                .ToList();

            return View(objSakList); // Veriyi Index.cshtml'e gönderiyoruz
        }

        public IActionResult FerdigeSaker()
        {
            List<SakModel> objSakList = _context.SakModels
                  .Where(s => s.Status == SakStatus.Ferdigstilt || s.Status == SakStatus.Avsluttet)
                  .ToList();

            return View(objSakList);
        }

        public IActionResult Detaljer(int id)
        {
            // Sak kaydını ve ilişkili ApplicationUser'ı al
            var sak = _context.SakModels
                .Include(s => s.ApplicationUser) // ApplicationUser'ı dahil et
                .FirstOrDefault(s => s.Id == id);

            if (sak == null)
            {
                return NotFound();
            }

            // Kontrolleren rolünün ID'sini bul
            var kontrollerenRoleId = _context.Roles
                .Where(r => r.Name == UserRoles.Role_Kontrolleren)
                .Select(r => r.Id)
                .FirstOrDefault();

            // Kontrolleren rolüne sahip kullanıcıları bul ve SelectListItem olarak hazırla
            var kontrollerenUsers = _context.Users
                .Where(u => _context.UserRoles
                    .Any(ur => ur.UserId == u.Id && ur.RoleId == kontrollerenRoleId))
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName,
                    Selected = u.Id == sak.AssignedKontrollerenId // Seçili durumu ayarlanıyor
                })
                .ToList();

            // Kontrolleren kullanıcılarını ViewBag'e ekle
            ViewBag.KontrollerenUsers = new SelectList(kontrollerenUsers, "Value", "Text", sak.AssignedKontrollerenId);

            // Status ve ArbeidStatus'u formatlayarak ViewBag'e ekle
            ViewBag.FormattedStatus = FormatStatus(sak.Status.ToString());
            ViewBag.FormattedArbeidStatus = FormatStatus(sak.ArbeidStatus.ToString());

            return View(sak);
        }

        // Status ve Arbeid Status Güncelleme İşlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id, string sakStatus, string arbeidStatus, string kontrollerenId, string saksBehandlerDescription)
        {
            Console.WriteLine("id değeri: " + id);
            SakModel? sakModel = _context.SakModels.FirstOrDefault(s => s.Id == id);

            if (sakModel == null)
            {
                return NotFound();
            }

            // SakStatus güncelleme
            if (Enum.TryParse(sakStatus, out SakStatus newSakStatus))
            {
                sakModel.Status = newSakStatus;
            }

            // ArbeidStatus güncelleme
            if (Enum.TryParse(arbeidStatus, out ArbeidStatus newArbeidStatus))
            {
                sakModel.ArbeidStatus = newArbeidStatus;
            }

            // Kontrolleren atanması
            if (!string.IsNullOrEmpty(kontrollerenId))
            {
                sakModel.AssignedKontrollerenId = kontrollerenId;
            }

            // Yeni yorumu güncelle
            sakModel.SaksBehandlerDescription = saksBehandlerDescription;

            _context.SaveChanges(); // Değişiklikleri kaydet
            return RedirectToAction("Detaljer", new { id }); // Güncelleme sonrası Detaljer sayfasına dön
        }

        // Profil sayfası - Kullanıcının profil bilgilerini görüntüleme ve güncelleme
        [HttpGet]
        public async Task<IActionResult> Profil()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.OfType<ApplicationUser>().FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Profil(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var existingUser = await _context.Users.OfType<ApplicationUser>().FirstOrDefaultAsync(u => u.Id == userId);

                if (existingUser != null)
                {
                    // Kullanıcı bilgilerini aynı şekilde girmişse uyarı ver
                    bool isSameData =
                        existingUser.Name == model.Name &&
                        existingUser.Surname == model.Surname &&
                        existingUser.Email == model.Email &&
                        existingUser.PhoneNumber == model.PhoneNumber &&
                        existingUser.Adresse == model.Adresse;


                    if (isSameData)
                    {
                        TempData["WarningMessage"] = "Du har skrevet inn de samme opplysningene, vennligst gjør en endring.";
                        return View(model);
                    }

                    // Bilgi boş ise uyarı ver
                    if (string.IsNullOrWhiteSpace(model.Name) ||
                        string.IsNullOrWhiteSpace(model.Surname) ||
                        string.IsNullOrWhiteSpace(model.Email) ||
                        string.IsNullOrWhiteSpace(model.Adresse) ||
                        string.IsNullOrWhiteSpace(model.PhoneNumber))
                    {
                        TempData["WarningMessage"] = "Vennligst fyll ut alle feltene.";
                        return View(model);
                    }

                    // Kullanıcı bilgilerini güncelle
                    existingUser.Name = model.Name;
                    existingUser.Surname = model.Surname;
                    existingUser.Email = model.Email;
                    existingUser.Adresse = model.Adresse;
                    existingUser.PhoneNumber = model.PhoneNumber;

                    _context.Users.Update(existingUser);
                    await _context.SaveChangesAsync();

                    // Başarılı güncelleme sonrası TempData kullanarak mesaj göster
                    TempData["ProfileUpdated"] = true;
                }

                return RedirectToAction("Profil");
            }

            // Model doğrulama hataları durumunda model ile birlikte sayfayı geri döndür
            TempData["WarningMessage"] = "Det oppstod valideringsfeil under oppdateringen.";
            return View(model);
        }


        // String formatlama fonksiyonu
        private string FormatStatus(string status)
        {
            return Regex.Replace(status, "([a-z])([A-Z])", "$1 $2");
        }
    }
}
