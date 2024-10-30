using Microsoft.AspNetCore.Mvc;
using KartApplication.Models;
using System;
using KartApplication.Data;
using Microsoft.AspNetCore.Authorization;

namespace KartApplication.Controllers
{
    [Authorize(Roles = UserRoles.Role_Saksbehandler)]

    public class SaksbehandlerController : Controller
    {
        // Saksbehandler giriş sayfası
        public IActionResult Index()
        {
          
            // Modeli View'e gönderiyoruz
            return View(); // Modeli Index.cshtml'e yolluyoruz
        }

        public IActionResult FerdigeSaker(){
            return View();

        }
        // Detay sayfası
        public IActionResult Detaljer(string id)
        {
            return View(); // Detay.cshtml'e model yollanıyor
        }

          // Profil sayfası
        public IActionResult Profil(string id)
        {
            return View(); // Profil.cshtml'e model yollanıyor
        }
    }
}
