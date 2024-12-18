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

    // GET: Se Bruker/Oversikt-siden
    [HttpGet]
    public IActionResult Oversikt()
    {
        return View(lastChange);
    }

    // Get: Bruker/Kvittering - Vis post ved bruk av referansenummer
    [HttpGet]
    public IActionResult Kvittering(string id)
    {
        if (lastChange != null && lastChange.Id == id)
        {
            return View(lastChange); // Vis om lastChange er full av dynamiske data
        }

        return NotFound(); // Vis feil hvis lastChange ikke ble funnet
    }

    // POST: Omdirigere til Bruker/Kvittering-siden
    [HttpPost]
    public IActionResult Kvittering()
    {
        if (lastChange != null)
        {
            return View(lastChange); // Vis lagret referanse
        }

        return NotFound(); // Vis feil hvis lastChange er tom
    }
}
