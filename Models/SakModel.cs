using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KartApplication.Models
{
    public class SakModel
    {
        [Key]
        public int Id { get; set; }

        public string? Address { get; set; }
        public string? GeoJson { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string SelectedMapType { get; set; }

        public bool IsTemporary { get; set; } = true;  // Varsayılan olarak geçici kaydediliyor


        public List<Coordinate> Coordinates { get; set; }

        public SakModel()
        {
            Coordinates = new List<Coordinate>();


        }




        public string? UserId { get; set; }  // Foreign key

        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }  // Navigation property

        ////public ICollection<FeedbackModel>? FeedbackModels { get; set; }



        //SakStatus enum'ı eklendi
        public SakStatus Status { get; set; } = SakStatus.SakMottatt;

        
        public ArbeidStatus ArbeidStatus { get; set; }
        
        
        
        
        
        
        //// SakStatus değiştiğinde mesaj gönderme fonksiyonu
        //public void ChangeStatus(SakStatus newStatus)
        //{
        //    if (newStatus != Status) // Eğer status değiştiyse
        //    {
        //      Status = newStatus;

        //       // Yeni mesajı al
        //       string message = SakStatusHelper.GetMessage(newStatus);

        //        // Geri bildirim ekle
        //       FeedbackModels?.Add(new FeedbackModel
        //        {
        //            SakId = this.Id,
        //            Message = message,
        //            SakModel = this
        //        });
        //    }
        //} 
    }

    public class Coordinate
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
