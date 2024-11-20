using Microsoft.AspNetCore.Mvc;
using KartApplication.Models;
using System;
using KartApplication.Data;
using Microsoft.AspNetCore.Authorization;

public class BrukerController : Controller
{
  

    private static AreaChange lastChange = null;

    // GET: Se Bruker/Indeksskjema
    [HttpGet]
    public IActionResult OpprettSak()
    {
        return View();
    }

    // POST: Behandle GeoJson, adresse og beskrivelse fra bruker
    [HttpPost]
    public IActionResult OpprettSak(string geoJson, string description, string address)
    {
        // Yeni 8 haneli numeric ID oluştur
        string newId = IdGenerator.GenerateNumericIdFromGuid();

        // AreaChange nesnesini doldur
        lastChange = new AreaChange
        {
            Id = newId,
            GeoJson = geoJson,
            Description = description,
            Address = address,
            Dato = DateTime.Now // Tarih burada atanıyor
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

    // GET: Bruker/Kvittering - Referans numarasını kullanarak kaydı göster
    [HttpGet]
    public IActionResult Kvittering(string id)
    {
        if (lastChange != null && lastChange.Id == id)
        {
            return View(lastChange); // lastChange dinamik verilerle doluysa göster
        }

        return NotFound(); // Eğer lastChange bulunamazsa hata göster
    }

    // POST: Bruker/Kvittering sayfasına yönlendir
    [HttpPost]
    public IActionResult Kvittering()
    {
        if (lastChange != null)
        {
            return View(lastChange); // Kaydedilen başvuruyu göster
        }

        return NotFound(); // Eğer lastChange boşsa hata göster
    }
}
