using Microsoft.AspNetCore.Mvc;
using KartApplication.Models;
using System;

public class BrukerController : Controller  // HomeController yerine BrukerController
{
    private static AreaChange lastChange = null;

    // GET: Bruker/Index formunu görüntüle
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    // POST: Kullanıcıdan gelen GeoJson, adres ve açıklamayı işleme al
    [HttpPost]
    public IActionResult Index(string geoJson, string description, string address)
    {
        // Yeni 8 haneli numeric ID oluştur
        string newId = IdGenerator.GenerateNumericIdFromGuid();

        // AreaChange nesnesini doldur
        lastChange = new AreaChange
        {
            Id = newId,
            GeoJson = geoJson,
            Description = description,
            Address = address
        };

        // Kayıt sonrası beskrivelse sayfasına yönlendir
        return RedirectToAction("Beskrivelse");
    }

    // GET: Bruker/Beskrivelse sayfasını göster
    [HttpGet]
    public IActionResult Beskrivelse()
    {
        return View(lastChange);
    }

    // POST: Beskrivelse sayfasındaki açıklamayı işleyip Oversikt'e gönder
    [HttpPost]
    public IActionResult Beskrivelse(string description)
    {
        if (lastChange != null)
        {
            lastChange.Description = description;
        }
        return RedirectToAction("Oversikt");
    }

    // GET: Bruker/Oversikt sayfasını görüntüle
    [HttpGet]
    public IActionResult Oversikt()
    {
        return View(lastChange);
    }

    // POST: Bruker/Kvittering sayfasına yönlendir
    [HttpPost]
    public IActionResult Kvittering()
    {
        return View(lastChange);
    }
}
