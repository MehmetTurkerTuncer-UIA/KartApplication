using Microsoft.AspNetCore.Identity;

namespace KartApplication.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }

        public string? Surname {  get; set; }
	
	    public string? Adresse {  get; set; }

        // Navigation property for the relationship
        //public ICollection<SakModel>? SakModels { get; set; }
    }

}