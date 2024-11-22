using Microsoft.AspNetCore.Mvc;
using KartApplication.Models;
using System;

namespace KartApplication.Controllers
{
    public class GjestController : Controller
    {
        private static AreaChange lastChange = null;

        // GET: Gjest/Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: Behandle GeoJson, adresse og beskrivelse fra bruker
        [HttpPost]
        public IActionResult Index(string geoJson, string description, string address)
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

        // GET: Vis Gjest/Beskrivelse side
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

        // GET: Gjest/Oversikt sayfasını görüntüle
        [HttpGet]
        public IActionResult Oversikt()
        {
            return View(lastChange);
        }

        // GET: Gjest/Kvittering - Referans numarasını kullanarak kaydı göster
        [HttpGet]
        public IActionResult Kvittering(string id)
        {
            if (lastChange != null && lastChange.Id == id)
            {
                return View(lastChange); // lastChange dinamik verilerle doluysa göster
            }

            return NotFound(); // Eğer lastChange bulunamazsa hata göster
        }

        // POST: Gjest/Kvittering sayfasına yönlendir
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
}
