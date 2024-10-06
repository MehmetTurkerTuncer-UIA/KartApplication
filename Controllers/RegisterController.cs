using Microsoft.AspNetCore.Mvc;

namespace KartApplication.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }


        //[HttpPost]
        //public IActionResult Register()
        //{

        //    return View();
        //}

    }
}
