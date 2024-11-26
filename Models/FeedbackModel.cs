namespace KartApplication.Models
{
    public class FeedbackModel
    {
        public int Id { get; set; }
        public string? Message { get; set; }  // For å lagre meldinger

        //public int SakId { get; set; }  // Foreign key to SakModel
        //public SakModel? SakModel { get; set; }  // Navigation property
    }
}
