using System.ComponentModel.DataAnnotations;

namespace KartApplication.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        
        public required string Name { get; set; }

        // Obligatorisk e-postadresse
        public required string Email { get; set; }
        [Required]
        public decimal Latitude { get; set; }
        [Required]
        public decimal Longitude { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

