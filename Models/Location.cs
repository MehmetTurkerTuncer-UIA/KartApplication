using System.ComponentModel.DataAnnotations;

namespace KartApplication.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        
        // Obligatorisk navn på lokasjon
        public required string Name { get; set; }

        // Obligatorisk e-postadresse
        public required string Email { get; set; }

        [Required]
        // Breddegrad (latitude) for lokasjon
        public decimal Latitude { get; set; }

        [Required]
        // Lengdegrad (longitude) for lokasjon
        public decimal Longitude { get; set; }

        // Dato og tid for når lokasjonen ble opprettet
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
