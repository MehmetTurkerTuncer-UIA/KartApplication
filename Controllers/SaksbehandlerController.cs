using Microsoft.AspNetCore.Mvc;
using KartApplication.Models;
using KartApplication.Data;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KartApplication.Controllers
{
    //[Authorize(Roles = UserRoles.Role_Saksbehandler)]
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
            var sak = _context.SakModels.FirstOrDefault(s => s.Id == id);
            if (sak == null)
            {
                return NotFound();
            }

            var kontrollerenRoleId = _context.Roles
                .Where(r => r.Name == UserRoles.Role_Kontrolleren)
                .Select(r => r.Id)
                .FirstOrDefault();

            var kontrollerenUsers = _context.Users
                .Where(u => _context.UserRoles
                    .Any(ur => ur.UserId == u.Id && ur.RoleId == kontrollerenRoleId))
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName,
                    Selected = u.Id == sak.AssignedKontrollerenId  // selected durumu burada ayarlanıyor
                })
                .ToList();

            ViewBag.KontrollerenUsers = new SelectList(kontrollerenUsers, "Value", "Text", sak.AssignedKontrollerenId);
            return View(sak);
        }

        // Status ve Arbeid Status Güncelleme İşlemi
        [HttpPost]
        public IActionResult UpdateStatus(int id, string sakStatus, string arbeidStatus, string kontrollerenId)
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


