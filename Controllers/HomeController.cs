using Microsoft.AspNetCore.Mvc;
using KartApplication.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KartApplication.Data;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Home/Index
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    // POST: Home/Login
    [HttpPost]
    public async Task<IActionResult> Login(UserRegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            // Eğer ModelState geçersizse, konsolda hangi alanların geçersiz olduğunu yazdır
            Console.WriteLine("ModelState geçersiz.");
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"Hata: {error.ErrorMessage}");
                }
            }
            // Geçersiz model verilerini tekrar formda göster
            return View("Index", model);
        }

        if (ModelState.IsValid)
        {
            // Kullanıcıyı veritabanında kontrol et
            var user = await _context.UserRegister
                .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

            if (user != null)
            {
                Console.WriteLine("Giriş başarılı, yönlendirme yapılıyor.");
                // Giriş başarılı, kullanıcıyı yönlendir
                return RedirectToAction("AreaChangeOverview", "Bruker");
            }
            ModelState.AddModelError("", "Geçersiz giriş bilgileri.");
            Console.WriteLine("Kullanici bulunamadi");
            // Kullanıcı bulunamazsa hata mesajı ekle
            ModelState.AddModelError("", "Geçersiz giriş bilgileri.");
        }
        Console.WriteLine("Giriş başarısiz, yönlendirme yapılıyor.");
        // Giriş başarısız olursa Index (login) sayfasını geri göster
        return View("Index", model);
    }

    //// GET: Home/Dashboard (Giriş sonrası yönlendirilecek sayfa)
    //[HttpGet]
    //public IActionResult Dashboard()
    //{
    //    return View();
    //}

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
