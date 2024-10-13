using System.ComponentModel.DataAnnotations;

namespace KartApplication.Models
{
    public class UserRegister
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Navn er påkrevd.")]
        [StringLength(100, ErrorMessage = "Navn må være mellom 2 og 100 tegn.", MinimumLength = 2)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "E-post er påkrevd.")]
        [EmailAddress(ErrorMessage = "Ugyldig e-postadresse.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Passord er påkrevd.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Passordet må være minst 6 tegn langt.", MinimumLength = 6)]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Bekreft passord er påkrevd.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passordene samsvarer ikke.")]
        public required string ConfirmPassword { get; set; }
    }
}
