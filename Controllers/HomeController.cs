using Microsoft.AspNetCore.Mvc;
using KartApplication.Models;
using System.Diagnostics;

public class HomeController : Controller
{
    // Eğer veri tabanı bağlantısı gerekiyorsa, aşağıdaki satırları kullanabilirsiniz.
    // private readonly ApplicationDbContext _context;

    // public HomeController(ApplicationDbContext context)
    // {
    //     _context = context;
    // }



    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult CorrectMap()
    {
        return View();
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