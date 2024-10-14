using Microsoft.EntityFrameworkCore;
using KartApplication.Models;


namespace KartApplication.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
                

        public DbSet<UserRegisterModel> UserRegister { get; set; }


    }
}
