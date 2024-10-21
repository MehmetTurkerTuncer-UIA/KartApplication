using Microsoft.AspNetCore.Mvc;
using KartApplication.Models;
using System;

namespace KartApplication.Controllers
{
    public class AdminController : Controller
    {
        // Saksbehandler giriş sayfası
        public IActionResult Index()
        {
          
            // Modeli View'e gönderiyoruz
            return View(); // Modeli Index.cshtml'e yolluyoruz
        }

        public IActionResult LeggtilAnsatt(){
            return View();

        }
        // Detay sayfası
        public IActionResult Oppdatere(string id)
        {
            return View(); // Detay.cshtml'e model yollanıyor
        }

    }
}
