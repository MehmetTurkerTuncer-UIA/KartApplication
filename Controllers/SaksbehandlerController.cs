using Microsoft.AspNetCore.Mvc;
using KartApplication.Models;
using KartApplication.Data;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

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
            Console.WriteLine("id değeri: " + id);

            var sak = _context.SakModels.FirstOrDefault(s => s.Id == id);
            if (sak == null)
            {
                return NotFound();
            }
            return View(sak); // SakModel nesnesini Detaljer görünümüne gönderiyoruz
        }

        // Status ve Kontroll Status Güncelleme İşlemi
        [HttpPost]
        public IActionResult UpdateStatus(int id, string sakStatus, string arbeidStatus)
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


