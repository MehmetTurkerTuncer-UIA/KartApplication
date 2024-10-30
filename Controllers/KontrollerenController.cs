using Microsoft.AspNetCore.Mvc;
using KartApplication.Models;
using System;
using KartApplication.Data;
using Microsoft.AspNetCore.Authorization;

namespace KartApplication.Controllers
{
    [Authorize(Roles = UserRoles.Role_Kontrolleren)]

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
