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
                

        
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        //public DbSet<FeedbackModel> FeedbackModel { get; set; }


        //
        //
        //  public DbSet<SakModel> SakModels { get; set; }

        //     protected override void OnModelCreating(ModelBuilder modelBuilder)
        //    {
        //////// SakStatus enum'ını string olarak saklamak için
        //        modelBuilder.Entity<SakModel>()
        //           .Property(s => s.Status)
        //            .HasConversion(
        //                v => v.ToString(),    // Enum'ı string'e çevir
        //                v => (SakStatus)Enum.Parse(typeof(SakStatus), v) // String'i tekrar Enum'a çevir
        //            );

        //        base.OnModelCreating(modelBuilder);
        //   }


    }
}
