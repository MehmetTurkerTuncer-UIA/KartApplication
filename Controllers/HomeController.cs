using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using KartApplication.Data;
using KartApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace KartApplication.Controllers
{
    [Authorize(Roles = UserRoles.Role_Bruker + "," + UserRoles.Role_Admin + "," + UserRoles.Role_Saksbehandler + "," + UserRoles.Role_Kontrolleren)]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Index-siden - Brukeren legger inn kartdata og adresseinformasjon.
        [HttpGet]
        public IActionResult Index()
        {
            var model = new SakModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(SakModel model)
        {
            if (ModelState.IsValid)
            {
                model.IsTemporary = true;  // Merk som midlertidig registrering.
                model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Bruker-ID blir tildelt.
                model.CreatedAt = DateTime.Now;

                _context.SakModels.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Beskrivelse", new { id = model.Id });
            }
            return View(model);
        }

        // Beskrivelse-siden - Brukeren legger til en beskrivelse.
        [HttpGet]
        public async Task<IActionResult> Beskrivelse(int id)
        {
            var model = await _context.SakModels.FindAsync(id);

            if (model == null || model.IsTemporary == false)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Beskrivelse(SakModel model)
        {
            if (ModelState.IsValid)
            {
                var existingModel = await _context.SakModels.FindAsync(model.Id);
                if (existingModel != null)
                {
                    existingModel.Description = model.Description;
                    existingModel.IsTemporary = true;
                    _context.SakModels.Update(existingModel);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Oversikt", new { id = model.Id });
            }
            return View(model);
        }

        // Oversikt-siden - Gjennomgang og godkjenning av all informasjon.
        [HttpGet]
        public async Task<IActionResult> Oversikt(int id)
        {
            var model = await _context.SakModels.FindAsync(id);

            if (model == null || model.IsTemporary == false)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitOversikt(int id)
        {
            var model = await _context.SakModels.FindAsync(id);

            if (model == null)
                return NotFound();

            // Hvis referansenummeret ikke allerede er opprettet, generer et nytt.
            if (string.IsNullOrEmpty(model.ReferenceNumber))
            {
                model.ReferenceNumber = GenerateUniqueReferenceNumber();
            }

            // Fjern midlertidig markering og gjør det permanent.

            model.IsTemporary = false;
            await _context.SaveChangesAsync();

            return RedirectToAction("Kvittering", new { id = model.Id });
        }


        // Metode for å generere et unikt referansenummer.
        private string GenerateUniqueReferenceNumber()
        {
            var random = new Random();
            string referenceNumber;
            do
            {
                referenceNumber = random.Next(10000000, 99999999).ToString(); // 8 haneli numara üret
            } while (_context.SakModels.Any(s => s.ReferenceNumber == referenceNumber)); // Eşsiz olduğundan emin ol

            return referenceNumber;
        }

        // Kvittering-siden - Vise bekreftelsessiden og sammendragsinformasjon.
        [HttpGet]
        public async Task<IActionResult> Kvittering(int id)
        {
            var model = await _context.SakModels
                .Include(s => s.ApplicationUser)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        // Metode for å slette midlertidige registreringer (kan brukes med en valgfri tidsplan).
        public async Task DeleteTemporaryRecords()
        {
            var temporaryRecords = await _context.SakModels
                .Where(s => s.IsTemporary == true)
                .ToListAsync();

            _context.SakModels.RemoveRange(temporaryRecords);
            await _context.SaveChangesAsync();
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

        // Minesaker Action Method - Kullanıcının tüm Sak kayıtlarını listeler
        [HttpGet]
        public async Task<IActionResult> Minesaker()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userSaks = await _context.SakModels
                .Where(s => s.UserId == userId)
                .ToListAsync();

            return View(userSaks);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSak(int id)
        {
            var sak = await _context.SakModels.FindAsync(id);
            if (sak == null)
            {
                return NotFound();
            }

            _context.SakModels.Remove(sak);
            await _context.SaveChangesAsync();

            return RedirectToAction("Minesaker");
        }

        // Detaljer Action Method - Belirli bir Sak kaydının detaylarını gösterir
        [HttpGet]
public async Task<IActionResult> Detaljer(int id)
{
    var sak = await _context.SakModels
        .Include(s => s.ApplicationUser) // ApplicationUser'ı dahil et
        .FirstOrDefaultAsync(s => s.Id == id);

    if (sak == null)
    {
        return NotFound();
    }

    return View(sak);
}


        // GET: Home/Sok
        public ActionResult Sok(string referenceNumber)
        {
            var status = CheckStatus(referenceNumber);

            ViewBag.Status = status;
            ViewBag.ReferenceNumber = referenceNumber;

            return View();
        }

        private string CheckStatus(string referenceNumber)
        {
            if (string.IsNullOrEmpty(referenceNumber))
            {
                return "Geçersiz referans numarası.";
            }

            return "Durum: Aktif";
        }
    }
}
