
namespace KartApplication.Models
{
    public class SakModel
    {
        public int Id { get; set; }

        public string? Address { get; set; }
        public string? GeoJson { get; set; }
        
        
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }

        // Foreign key for ApplicationUser
        public int UserId { get; set; }  // Foreign key to InputUserModel
        public ApplicationUser? ApplicationUser { get; set; }  // Navigation property

    }
}
