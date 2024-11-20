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
        // Opprett ny 8-sifret numerisk ID
        string newId = IdGenerator.GenerateNumericIdFromGuid();

        // Fyll ut AreaChange-objekt
        lastChange = new AreaChange
        {
            Id = newId,
            GeoJson = geoJson,
            Description = description,
            Address = address,
            Dato = DateTime.Now // Dato er oppgitt her
        };

        // Omdiriger til beskrivelsessiden etter registrering
        return RedirectToAction("Beskrivelse");
    }

    // GET: Vis Bruker/Beskrivelse side
    [HttpGet]
    public IActionResult Beskrivelse()
    {
        return View(lastChange);
    }

    // POST: Behandle beskrivelsen på Beskrivelse-siden og send den til Oversikt
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
