using KartApplication.Data;
using KartApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KartApplication.Controllers
{
    public class UserRegisterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserRegisterController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserRegister/UserRegister
        [HttpGet]
        public IActionResult UserRegister()
        {
            // View adı 'UserRegister' olarak belirtildi
            return View("UserRegister");
        }

        // POST: UserRegister/UserRegister
        [HttpPost]
        public async Task<IActionResult> UserRegister(UserRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Yeni kullanıcı oluşturma işlemi
                var user = new UserRegisterModel
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password, // Parola hashlenmelidir!
                    ConfirmPassword = model.ConfirmPassword
                };

                // Kullanıcıyı veritabanına ekle
                _context.UserRegister.Add(user);
                await _context.SaveChangesAsync();

                // Kayıt sonrası bir sayfaya yönlendirme
                return RedirectToAction("Index", "Home");
            }

            // Model geçerli değilse formu tekrar göster
            return View("UserRegister", model);
        }

        // GET: UserRegister/RegisterSuccess
        [HttpGet]
        public IActionResult RegisterSuccess()
        {
            return View("Index", "Home");
        }
    }
}
