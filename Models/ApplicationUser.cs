using Microsoft.AspNetCore.Identity;



namespace KartApplication.Models
{ 

public class ApplicationUser : IdentityUser
    {
	 public string? Surname {  get; set; }
	
	public string? Adresse {  get; set; }
}

}