using Microsoft.AspNetCore.Mvc;

namespace KartApplication.Controllers
{
    public class BrukerController : Controller
    {
        public IActionResult Index()
        {
            return View("Bruker");
        }
    }
}
