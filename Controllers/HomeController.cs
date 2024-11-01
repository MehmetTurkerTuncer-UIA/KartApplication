﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using KartApplication.Data;
using KartApplication.Models;
using Microsoft.AspNetCore.Authorization;

namespace KartApplication.Controllers
{
    //[Authorize(Roles = UserRoles.Role_Bruker + "," + UserRoles.Role_Admin + "," + UserRoles.Role_Saksbehandler + "," + UserRoles.Role_Kontrolleren)]

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

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
            // Index'te kaydedilen verileri almak için veritabanından id ile sorgulama yapılır
            var model = await _context.SakModels.FindAsync(id);

            // Model null ya da geçici değilse hata döndür
            if (model == null || model.IsTemporary == false)
                return NotFound();

            return View(model); // Modeli Beskrivelse sayfasına aktar
        }

        [HttpPost]
        public async Task<IActionResult> Beskrivelse(SakModel model)
        {
          
            if (ModelState.IsValid)
            {
                // Mevcut kaydı güncelle
                var existingModel = await _context.SakModels.FindAsync(model.Id);
                if (existingModel != null)
                {
                    existingModel.Description = model.Description; // Kullanıcının girdiği açıklama kaydediliyor
                    existingModel.IsTemporary = true; // Geçici olarak işaretle
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
            // Veritabanından id ile kaydı bul
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

            // Geçici işareti kaldırarak kalıcı hale getir
            model.IsTemporary = false;
            await _context.SaveChangesAsync();

            return RedirectToAction("Kvittering", new { id = model.Id });
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

        // Profil sayfası - Kullanıcının profil bilgilerini görüntüleme ve güncelleme
[HttpGet]
public async Task<IActionResult> Profil()
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var user = await _context.Users.FindAsync(userId);

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
        var existingUser = await _context.Users.FindAsync(userId);

        if (existingUser != null)
        {
            // Kullanıcı bilgilerini güncelle
            existingUser.UserName = model.Name;
            existingUser.Email = model.Email;
            existingUser.PhoneNumber = model.PhoneNumber;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Profil");
    }

    return View(model);
}

[HttpGet]
        public IActionResult MineSaker()
        {
            return View();
        }

    }
}
