using Microsoft.EntityFrameworkCore;
using KartApplication.Models;


namespace KartApplication.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

         //Veritabanındaki Locations tablosunu temsil eden DbSet
       // public DbSet<Location> LocationsTable { get; set; }

        public DbSet<Users> UsersTable { get; set; }

       

    }
}

