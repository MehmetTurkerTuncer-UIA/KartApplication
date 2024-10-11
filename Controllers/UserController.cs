using KartApplication.Data;
using KartApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace KartApplication.Controllers
{
    public class UserController : Controller
    {
        
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /User/Index
        public IActionResult Index()
        {
            var users = _context.Users.ToList(); // Veritabanındaki tüm kullanıcıları listele
            return View(users);
        }

        // GET: /User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user); // Yeni kullanıcı ekle
                await _context.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
    }
}
