using Microsoft.AspNetCore.Mvc;
using KartApplication.Models;
using KartApplication.Data;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace KartApplication.Controllers
{
    [Authorize(Roles = UserRoles.Role_Saksbehandler)]
    public class SaksbehandlerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SaksbehandlerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Saksbehandler giriş sayfası
        public IActionResult Index()
        {
            List<SakModel> objSakList = _context.SakModels
                .Where(s => s.Status == SakStatus.SakMottatt || s.Status == SakStatus.UnderBehandling)
                .ToList();

           
            return View(objSakList); // Veriyi Index.cshtml'e gönderiyoruz

           
        }

        public IActionResult FerdigeSaker()
        {
            List<SakModel> objSakList = _context.SakModels
                  .Where(s => s.Status == SakStatus.Ferdigstilt || s.Status == SakStatus.Avsluttet)
                  .ToList();


            return View(objSakList);
                       
        }

     public IActionResult Detaljer(int id)
{
    // Sak kaydını ve ilişkili ApplicationUser'ı al
    var sak = _context.SakModels
        .Include(s => s.ApplicationUser) // ApplicationUser'ı dahil et
        .FirstOrDefault(s => s.Id == id);

    if (sak == null)
    {
        return NotFound();
    }

    // Kontrolleren rolünün ID'sini bul
    var kontrollerenRoleId = _context.Roles
        .Where(r => r.Name == UserRoles.Role_Kontrolleren)
        .Select(r => r.Id)
        .FirstOrDefault();

    // Kontrolleren rolüne sahip kullanıcıları bul ve SelectListItem olarak hazırla
    var kontrollerenUsers = _context.Users
        .Where(u => _context.UserRoles
            .Any(ur => ur.UserId == u.Id && ur.RoleId == kontrollerenRoleId))
        .Select(u => new SelectListItem
        {
            Value = u.Id,
            Text = u.UserName,
            Selected = u.Id == sak.AssignedKontrollerenId // Seçili durumu ayarlanıyor
        })
        .ToList();

    // Kontrolleren kullanıcılarını ViewBag'e ekle
    ViewBag.KontrollerenUsers = new SelectList(kontrollerenUsers, "Value", "Text", sak.AssignedKontrollerenId);

    return View(sak);
}


        // Status ve Arbeid Status Güncelleme İşlemi
        [HttpPost]
        public IActionResult UpdateStatus(int id, string sakStatus, string arbeidStatus, string kontrollerenId, string saksBehandlerDescription)
        {
            Console.WriteLine("id değeri: " + id);
            SakModel? sakModel = _context.SakModels.FirstOrDefault(s => s.Id == id);

            if (sakModel == null)
            {
                return NotFound();
            }

            // SakStatus güncelleme
            if (Enum.TryParse(sakStatus, out SakStatus newSakStatus))
            {
                sakModel.Status = newSakStatus;
            }

            // ArbeidStatus güncelleme
            if (Enum.TryParse(arbeidStatus, out ArbeidStatus newArbeidStatus))
            {
                sakModel.ArbeidStatus = newArbeidStatus;
            }

            // Kontrolleren atanması
            if (!string.IsNullOrEmpty(kontrollerenId))
            {
                sakModel.AssignedKontrollerenId = kontrollerenId;
            }


            // Yeni yorumu güncelle
            sakModel.SaksBehandlerDescription = saksBehandlerDescription;


            _context.SaveChanges(); // Değişiklikleri kaydet
            return RedirectToAction("Detaljer", new { id }); // Güncelleme sonrası Detaljer sayfasına dön
        }


        // Profil sayfası
        public IActionResult Profil(string id)
        {
            return View(); // Profil.cshtml'e model yollanıyor
        }


    }
}


