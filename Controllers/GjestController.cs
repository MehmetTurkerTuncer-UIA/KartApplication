using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KartApplication.Models;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using KartApplication.Data;

namespace KartApplication.Controllers
{
    public class GjestController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly ApplicationDbContext _context;


        public GjestController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
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
                EmailConfirmed = true,
                Name = "Gjest"
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

        //// Gjest Index sayfası
        //[HttpGet]
        //public IActionResult Index()
        //{
        //    return View();
        //}

        //// Gjest Beskrivelse sayfası
        //[HttpPost]
        //public IActionResult Beskrivelse(SakModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Verileri işleme alabilir ve saklayabilirsiniz
        //        return RedirectToAction("Beskrivelse");
        //    }

        //    return View("Index", model);
        //}

        
        
        // Index sayfası - Kullanıcının harita verisi ve adres bilgisi girişi yapması
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
                model.IsTemporary = true;  // Geçici kayıt olarak işaretle
                model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Kullanıcı ID'si atanıyor
                model.CreatedAt = DateTime.Now;

                _context.SakModels.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Beskrivelse", new { id = model.Id });
            }
            return View(model);
        }


        // Beskrivelse sayfası - Kullanıcının açıklama eklemesi
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

        // Oversikt sayfası - Tüm bilgilerin gözden geçirilip onaylanması
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

            // Eğer referans numarası daha önce oluşturulmadıysa yeni bir tane üret
            if (string.IsNullOrEmpty(model.ReferenceNumber))
            {
                model.ReferenceNumber = GenerateUniqueReferenceNumber();
            }

            // Geçici işareti kaldırarak kalıcı hale getir

            model.IsTemporary = false;
            await _context.SaveChangesAsync();

            return RedirectToAction("Kvittering", new { id = model.Id });
        }


        // Benzersiz referans numarası oluşturma metodu
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

        // Kvittering sayfası - Onay sayfası ve özet bilgileri gösterme
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

        // Geçici Kayıtları Silme Metodu (isteğe bağlı zamanlayıcı ile kullanılabilir)
        public async Task DeleteTemporaryRecords()
        {
            var temporaryRecords = await _context.SakModels
                .Where(s => s.IsTemporary == true)
                .ToListAsync();

            _context.SakModels.RemoveRange(temporaryRecords);
            await _context.SaveChangesAsync();
        }



    }
}
