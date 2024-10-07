using KartApplication.Data;
using Microsoft.AspNetCore.Mvc;
using KartApplication.Models;


namespace KartApplication.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public  UsersController(ApplicationDbContext context)
        {
            _applicationDbContext = context;
        }
     public IActionResult Users()
        {
           // List<Users> objUsersList = _applicationDbContext.UsersTable.ToList();
            return View();
        }
    }
}
