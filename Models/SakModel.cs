﻿using System.ComponentModel.DataAnnotations;
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

        public string? ReferenceNumber { get; set; }

        public string? KontrollerenDescription { get; set; }

        public string? SaksBehandlerDescription { get; set; }


        public List<Coordinate> Coordinates { get; set; }

        public SakModel()
        {
            Coordinates = new List<Coordinate>();
          

        }




        public string? UserId { get; set; }  // Foreign key

        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }  // Navigation property


        public string? AssignedKontrollerenId { get; set; }  // Foreign key for assigned Kontrolleren
        [ForeignKey("AssignedKontrollerenId")]
        public ApplicationUser? AssignedKontrolleren { get; set; }  // Atanmış Kontrolleren bilgisi



        ////public ICollection<FeedbackModel>? FeedbackModels { get; set; }





        //SakStatus enum'ı eklendi
        public SakStatus Status { get; set; } = SakStatus.SakMottatt;


        public ArbeidStatus ArbeidStatus { get; set; } = ArbeidStatus.IkkeTilordnet;

        public KontrolStatus KontrolStatus { get; set; } = KontrolStatus.villKontrollere;
        
        
        
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
