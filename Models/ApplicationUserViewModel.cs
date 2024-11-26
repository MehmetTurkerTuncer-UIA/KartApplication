namespace KartApplication.Models
{
    internal class ApplicationUserViewModel
    {
        public string UserId { get; set; }  // Bruker-ID (User ID)
        public string UserName { get; set; }  // Brukernavn (Username)
        public string Email { get; set; }  // E-postadresse (Email)
        public string PhoneNumber { get; set; }  // Telefonnummer (Phone number)
        public string Name { get; set; }  // Fornavn (First name)
        public string Surname { get; set; }  // Etternavn (Surname)
        public string Adresse { get; set; }  // Adresse (Address)
        public string CurrentRole { get; set; }  // Nåværende rolle (Current role)
    }
}