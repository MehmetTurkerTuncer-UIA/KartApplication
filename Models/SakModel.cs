using System.ComponentModel.DataAnnotations;

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







        // Foreign key for ApplicationUser
        //public string? UserId { get; set; }  // Foreign key to InputUserModel
        //public ApplicationUser? ApplicationUser { get; set; }  // Navigation property

        ////public ICollection<FeedbackModel>? FeedbackModels { get; set; }



         //SakStatus enum'ı eklendi
        //public SakStatus Status { get; set; } = SakStatus.SakMottatt;

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
}
