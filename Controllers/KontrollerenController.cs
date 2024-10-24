using Microsoft.AspNetCore.Mvc;
using KartApplication.Models;
using System;

namespace KartApplication.Controllers
{
    public class KontrollerenController : Controller
    {
        // Saksbehandler giriş sayfası
        public IActionResult Index()
        {
          
            // Modeli View'e gönderiyoruz
            return View(); // Modeli Index.cshtml'e yolluyoruz
        }

        public IActionResult Detaljer(){
            return View();

        }

    }
}
