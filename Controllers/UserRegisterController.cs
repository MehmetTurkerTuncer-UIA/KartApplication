using KartApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KartApplication.Data;

namespace KartApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Register GET: Kullanıcı kayıt formunu gösterir.
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Register POST: Kullanıcıyı veritabanına kaydeder.
        [HttpPost]
        public async Task<IActionResult> Register(UserRegister model)
        {
            if (ModelState.IsValid)
            {
                // Yeni kullanıcı oluşturuluyor
                UserRegister newUser = new UserRegister
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password, // Şifreleme eklenmeli!
                    ConfirmPassword = model.ConfirmPassword
                };

                // Kullanıcı veritabanına ekleniyor
                _context.UserRegisterTable.Add(newUser);
                await _context.SaveChangesAsync();

                // Kullanıcı kaydedildikten sonra başka bir sayfaya yönlendiriliyor
                return RedirectToAction("Index", "Home");
            }

            return View(model); // Hatalar varsa tekrar formu göster
        }
    }
}
