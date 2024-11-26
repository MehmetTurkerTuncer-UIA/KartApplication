namespace KartApplication.Models
{
    public class AreaChange
    {
        public string Id { get; set; }
        public string GeoJson { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public DateTime Dato { get; set; }  // Dato for endring (Date of change)

        // Property to store selected map type
        public string SelectedMapType { get; set; } // "fargekart", "gratonekart", "turkart", "sjokart"
    }
}
