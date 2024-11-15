﻿using Microsoft.AspNetCore.Mvc;
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
                Address = address,
                Dato = DateTime.Now // Tarih burada atanıyor
            };

            // Kayıt sonrası beskrivelse sayfasına yönlendir
            return RedirectToAction("Beskrivelse");
        }

        // GET: Gjest/Beskrivelse sayfasını göster
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
