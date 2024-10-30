using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using KartApplication.Data;
using KartApplication.Models;
using System.Net;



//namespace KartApplication.Controllers


    public class HomeController2 : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController2(ApplicationDbContext context)
    {
        _context = context;
    }

    // Index Sayfası: Kullanıcı koordinatları seçer, GeoJson ve harita tipini kaydeder
    [HttpGet]
    public IActionResult Index()
    {
        // Eğer daha önce Session'da veri yoksa, yeni bir model oluşturuluyor
        var model = new SakModel
        {
            GeoJson = HttpContext.Session.GetString("GeoJson") ?? "",
            SelectedMapType = HttpContext.Session.GetString("SelectedMapType") ?? "defaultMapType",
            Address = HttpContext.Session.GetString("Address") ?? ""
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult Index(SakModel model)
    {
        if (ModelState.IsValid)
        {
            // Kullanıcının seçtiği verileri Session'a kaydediyoruz
            HttpContext.Session.SetString("GeoJson", model.GeoJson ?? "");
            HttpContext.Session.SetString("SelectedMapType", model.SelectedMapType ?? "defaultMapType");
            HttpContext.Session.SetString("Address", model.Address ?? "");

            return RedirectToAction("Beskrivelse");
        }
        return View(model);
    }

    // Beskrivelse Sayfası: Kullanıcı açıklama ekler, seçilen koordinatları haritada gösterir
    [HttpGet]
    public IActionResult Beskrivelse()
    {
        // Session'daki verileri modele alarak sayfada gösteriyoruz
        var model = new SakModel
        {
            GeoJson = HttpContext.Session.GetString("GeoJson") ?? "",
            SelectedMapType = HttpContext.Session.GetString("SelectedMapType") ?? "defaultMapType",
            Address = HttpContext.Session.GetString("Address") ?? "",
            };
        return View(model);
    }

    [HttpPost]
    public IActionResult Beskrivelse(SakModel model)
    {
        if (ModelState.IsValid)
            {
           // Kullanıcının girdiği açıklamayı Session'a kaydediyoruz
          
            HttpContext.Session.SetString("Description", model.Description ?? "");
            return RedirectToAction("Oversikt");
        }

            model.GeoJson = HttpContext.Session.GetString("GeoJson") ?? "";
            model.SelectedMapType = HttpContext.Session.GetString("SelectedMapType") ?? "defaultMapType";
            model.Address = HttpContext.Session.GetString("Address") ?? "";
            return View(model);
    }

    // Oversikt Sayfası: Kullanıcı tüm bilgileri doğrular, harita ve açıklama görünür
    [HttpGet]
    public IActionResult Oversikt()
    {
        // Session'daki tüm verileri modele alarak doğrulama için sayfada gösteriyoruz
        var model = new SakModel
        {
            GeoJson = HttpContext.Session.GetString("GeoJson") ?? "",
            SelectedMapType = HttpContext.Session.GetString("SelectedMapType") ?? "defaultMapType",
            Address = HttpContext.Session.GetString("Address") ?? "",
            Description = HttpContext.Session.GetString("Description") ?? ""
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitOversikt()
    {
        // Tüm verileri SakModel nesnesine atayıp veritabanına kaydediyoruz
        var model = new SakModel
        {
            GeoJson = HttpContext.Session.GetString("GeoJson") ?? "",
            SelectedMapType = HttpContext.Session.GetString("SelectedMapType") ?? "defaultMapType",
            Address = HttpContext.Session.GetString("Address") ?? "",
            Description = HttpContext.Session.GetString("Description") ?? "",
            CreatedAt = DateTime.Now,
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
        };

        // Veriyi veritabanına ekliyoruz
        _context.SakModels.Add(model);
        await _context.SaveChangesAsync();

        // Session'daki verileri temizliyoruz
        HttpContext.Session.Remove("GeoJson");
        HttpContext.Session.Remove("SelectedMapType");
        HttpContext.Session.Remove("Address");
        HttpContext.Session.Remove("Description");

        return RedirectToAction("Kvittering", new { id = model.Id });
    }

    // Kvittering Sayfası: Kullanıcıya onay ve özet bilgileri gösterir
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
}
