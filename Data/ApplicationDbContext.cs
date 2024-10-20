using Microsoft.EntityFrameworkCore;
using KartApplication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace KartApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
                

        public DbSet<UserRegisterModel> UserRegister { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<SakModel> SakModels { get; set; }


    }
}
