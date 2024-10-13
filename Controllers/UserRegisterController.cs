using Microsoft.AspNetCore.Mvc;

namespace KartApplication.Controllers
{
    public class UserRegisterController : Controller
    {
        public IActionResult UserRegister()
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
