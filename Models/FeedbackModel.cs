namespace KartApplication.Models
{
    public class FeedbackModel
    {
        public int Id { get; set; }
        public string? Message { get; set; }  // For å lagre meldinger

        //public int SakId { get; set; }  // Fremmednøkkel til SakModel
        //public SakModel? SakModel { get; set; }  // Navigasjonsegenskap
    }
}
