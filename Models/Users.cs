using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KartApplication.Models
{
    
    public class Users
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(20)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        //[StringLength(40, MinimumLength = 8, ErrorMessage = "The {0} must be at {2} and at max {1} characters long.")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "Password does not match")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "ConfirmPassword is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public required string ConfirmPassword { get; set; }
    }
}
