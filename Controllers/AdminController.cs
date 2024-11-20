using Microsoft.AspNetCore.Mvc;
using KartApplication.Data;
using KartApplication.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace KartApplication.Controllers
{
    [Authorize(Roles = UserRoles.Role_Admin )]

    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var userListWithRoles = new List<ApplicationUserViewModel>();

            foreach (var user in users)
            {
                // Få brukerens roller
                var roles = await _userManager.GetRolesAsync(user);
                var currentRole = roles.FirstOrDefault() ?? "Bruker"; // Varsayılan olarak "Bruker" atanabilir

                userListWithRoles.Add(new ApplicationUserViewModel
                {
                    UserId = user.Id,
                    UserName = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Name = user.Name,
                    Surname = user.Surname,
                    Adresse = user.Adresse,
                    CurrentRole = currentRole
                });
            }

            // Vi sender rollevalg via ViewBag
            ViewBag.RoleOptions = new SelectList(await _roleManager.Roles.Select(r => r.Name).ToListAsync());

            return View(userListWithRoles);
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, newRole);
            }
            return RedirectToAction("Index");
        }

        
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Sjekk brukerens Sak-poster
            var hasSakRecords = await _context.SakModels
                .AnyAsync(s => s.UserId == userId);

            if (hasSakRecords)
            {
                // Hvis det er lagrede poster, send varselmeldingen med ViewBag
                ViewBag.ErrorMessage = "Det er Sak-poster for denne brukeren. Slett først Sak-postene.";

                // Vi forbereder indeksvisningen for å laste inn listen over brukere og roller på nytt
                var users = _userManager.Users.ToList();
                var userListWithRoles = new List<ApplicationUserViewModel>();

                foreach (var usr in users)
                {
                    var roles = await _userManager.GetRolesAsync(usr);
                    var currentRole = roles.FirstOrDefault() ?? "Bruker";

                    userListWithRoles.Add(new ApplicationUserViewModel
                    {
                        UserId = usr.Id,
                        UserName = usr.Name,
                        Email = usr.Email,
                        PhoneNumber = usr.PhoneNumber,
                        Name = usr.Name,
                        Surname = usr.Surname,
                        Adresse = usr.Adresse,
                        CurrentRole = currentRole
                    });
                }

                ViewBag.RoleOptions = new SelectList(await _roleManager.Roles.Select(r => r.Name).ToListAsync());

            // Vi venter 5 sekunder og går tilbake til indeksvisningen                return View("Index", userListWithRoles);
            }

            // Slett brukeren hvis det ikke er noen lagringspost
            await _userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> BrukerSaker(string userId)
        {
            //Få bruker med UserManager
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(); // Returner feil hvis brukeren ikke ble funnet
            }

            // Kullanıcının Sak listesini DbContext üzerinden getirin

            ViewBag.UserName = user.Name;
            ViewBag.UserId = user.Id;

            var saker = await _context.SakModels
                .Where(s => s.UserId == userId)
                .ToListAsync();


           // Sak listesiyle birlikte sayfayı döndürün
           // return View(saker);

            // Sak listesiyle birlikte "BrukerSaker" görünümünü döndürün
            return View("BrukerSaker", saker);
        }

        public async Task<IActionResult> Detaljer(int id)
        {
            var sak = await _context.SakModels.FindAsync(id);
            return View(sak);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSak(int id, string userId)
        {
            var sak = await _context.SakModels.FindAsync(id);
            if (sak == null)
            {
                return NotFound(); // Sak bulunamazsa hata döndür
            }

            _context.SakModels.Remove(sak);
            await _context.SaveChangesAsync();

            // Sak silindikten sonra aynı kullanıcıya ait Sak listesini göstermek için BrukerSaker'a yönlendiriyoruz
            return RedirectToAction("BrukerSaker", new { userId = userId });
        }


    }



}
