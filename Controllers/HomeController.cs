using Microsoft.AspNetCore.Mvc;
using KartApplication.Data;
using KartApplication.Models;
using System.Diagnostics;

public class HomeController : Controller
{
    //private readonly ApplicationDbContext _context;

    //public HomeController(ApplicationDbContext context)
    //{
    //    _context = context;
    //}

    public IActionResult Index()
    {
        return View();
    
       
    }

[HttpPost]
public IActionResult CorrectModel(PositionModel model)
{
    if (ModelState.IsValid)
    {
        // Hata bildirimini bir listeye ekleyin veya veritabanına kaydedin
        positions.Add(model); // Bu, in-memory liste örneğidir
        return RedirectToAction("CorrectionOverview", positions);
    }
    return View();
}

// Veri Görüntüleme için Ek GET Metodu
    [HttpGet]
    public IActionResult CorrectionOverview()
    {
        // positions listesini CorrectionOverview görünümüne gönder
        return View(positions);
    }

    public IActionResult Privacy()
    {
        return View();
    }




    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
