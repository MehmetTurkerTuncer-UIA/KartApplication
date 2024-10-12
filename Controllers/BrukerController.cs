using Microsoft.AspNetCore.Mvc;
using KartApplication.Models;
using System;
using System.Collections.Generic;
using KartApplication.Helpers;

namespace KartApplication.Controllers
{
    public class BrukerController : Controller
    {
        // AreaChange kayıtlarını saklamak için bir liste
        private static List<AreaChange> changes = new List<AreaChange>();

        // GET: Bruker ana sayfasını göster
        public IActionResult Index()
        {
            return View("Bruker");
        }

        // GET: RegisterAreaChange sayfasını göster
        [HttpGet]
        public IActionResult RegisterAreaChange()
        {
            return View("RegisterAreaChange");
        }

        // POST: RegisterAreaChange formunu işle
        [HttpPost]
        public IActionResult RegisterAreaChange(string geoJson, string latitude, string longitude, string description, string address)
        {
            // Yeni bir AreaChange nesnesi oluştur ve bilgileri ata
            var newChange = new AreaChange
            {
                GeoJson = geoJson,
                Latitude = latitude,
                Longitude = longitude,
                Description = description,
                Address = address
            };

            // Beskrivelse sayfasına yönlendir ve modeli geç
            return RedirectToAction("BeskrivelseGet", newChange);
        }

        // GET: Beskrivelse sayfasını göster
        [HttpGet]
        public IActionResult BeskrivelseGet(AreaChange model)
        {
            // Model null ise yeni bir kayıt oluşturulacak
            if (model == null)
            {
                return RedirectToAction("RegisterAreaChange");
            }

            // Veriyi beskrivelse sayfasına gönder
            return View("Beskrivelse", model);
        }

        // POST: Beskrivelse formunu işle
        [HttpPost]
        public IActionResult BeskrivelsePost(string id, string geoJson, string latitude, string longitude, string description, string address)
        {
            // Gerekli alanların dolu olduğundan emin ol
            if (string.IsNullOrEmpty(description) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(latitude) || string.IsNullOrEmpty(longitude))
            {
                ModelState.AddModelError("", "Tüm alanlar zorunludur.");
                var model = new AreaChange
                {
                    GeoJson = geoJson,
                    Latitude = latitude,
                    Longitude = longitude,
                    Description = description,
                    Address = address
                };
                return View("Beskrivelse", model);
            }

            // Kayıt oluştur ve listeye ekle
            var originalGuid = Guid.NewGuid().ToString();
            var newChange = new AreaChange
            {
                Id = GuidHelper.ShortenGuid(originalGuid),
                GeoJson = geoJson,
                Latitude = latitude,
                Longitude = longitude,
                Description = description,
                Address = address
            };
            changes.Add(newChange);

            // Kaydedilen değişiklik ile birlikte AreaChangeOverview sayfasına yönlendir
            return RedirectToAction("AreaChangeOverview", new { id = newChange.Id });
        }

        // GET: AreaChangeOverview sayfasını göster
        [HttpGet]
        public ActionResult AreaChangeOverview(double latitude, double longitude, string address, string description)
        {
            var model = new AreaChange
            {
                Latitude = latitude.ToString(),
                Longitude = longitude.ToString(),
                Address = address,
                Description = description
            };

            return View("AreaChangeOverview", model);
        }
    }
}