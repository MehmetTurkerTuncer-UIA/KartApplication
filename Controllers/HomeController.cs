using Microsoft.AspNetCore.Mvc;
using KartApplication.Models;
using System.Diagnostics;

public class HomeController : Controller
{
    // Eğer veri tabanı bağlantısı gerekiyorsa, aşağıdaki satırları kullanabilirsiniz.
    // private readonly ApplicationDbContext _context;

    //public HomeController(ApplicationDbContext context)
    //{
    //    _context = context;
    //}

    //[HttpGet]
    public IActionResult Index()
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


      private static AreaChange lastChange = null;

    // GET: Bruker/Index formunu görüntüle
    [HttpGet]
    public IActionResult OpprettSak()
    {
        return View();
    }

    // POST: Kullanıcıdan gelen GeoJson, adres ve açıklamayı işleme al
    [HttpPost]
    public IActionResult OpprettSak(string geoJson, string description, string address)
    {
        // Yeni 8 haneli numeric ID oluştur
        string newId = IdGenerator.GenerateNumericIdFromGuid();

        // AreaChange nesnesini doldur
        lastChange = new AreaChange
        {
            Id = newId,
            GeoJson = geoJson,
            Description = description,
            Address = address,
            Dato = DateTime.Now // Tarih burada atanıyor
        };

        // Kayıt sonrası beskrivelse sayfasına yönlendir
        return RedirectToAction("Beskrivelse");
    }

    // GET: Bruker/Beskrivelse sayfasını göster
    [HttpGet]
    public IActionResult Beskrivelse()
    {
        return View(lastChange);
    }

    // POST: Beskrivelse sayfasındaki açıklamayı işleyip Oversikt'e gönder
    [HttpPost]
    public IActionResult Beskrivelse(string description)
    {
        if (lastChange != null)
        {
            lastChange.Description = description;
        }
        return RedirectToAction("Oversikt");
    }

    // GET: Bruker/Oversikt sayfasını görüntüle
    [HttpGet]
    public IActionResult Oversikt()
    {
        return View(lastChange);
    }

    // GET: Bruker/Kvittering - Referans numarasını kullanarak kaydı göster
    [HttpGet]
    public IActionResult Kvittering(string id)
    {
        if (lastChange != null && lastChange.Id == id)
        {
            return View(lastChange); // lastChange dinamik verilerle doluysa göster
        }

        return NotFound(); // Eğer lastChange bulunamazsa hata göster
    }

    // POST: Bruker/Kvittering sayfasına yönlendir
    [HttpPost]
    public IActionResult Kvittering()
    {
        if (lastChange != null)
        {
            return View(lastChange); // Kaydedilen başvuruyu göster
        }

        return NotFound(); // Eğer lastChange boşsa hata göster
    }

    
}