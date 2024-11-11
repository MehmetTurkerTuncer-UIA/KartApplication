using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using KartApplication.Data;
using KartApplication.Models;

namespace KartApplication.Controllers
{
    public class SokController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SokController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sok/Index
        [HttpGet]
        public async Task<IActionResult> Index(string referenceNumber)
        {
            if (string.IsNullOrEmpty(referenceNumber))
            {
                ViewBag.Status = "Geçersiz referans numarası.";
                return View(new List<SakModel>());
            }

            // Veritabanında referans numarasını arayın
            var sakRecords = await _context.SakModels
                .Where(s => s.ReferenceNumber == referenceNumber)
                .ToListAsync();

            if (sakRecords.Any())
            {
                ViewBag.Status = "Kayıt bulundu.";
            }
            else
            {
                ViewBag.Status = "Sisteminizde bu referans numarası bulunamadı.";
            }

            return View(sakRecords);
        }
    }
}
