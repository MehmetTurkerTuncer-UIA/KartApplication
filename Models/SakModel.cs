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

        public bool IsTemporary { get; set; } = true;  // Standard er at saken lagres midlertidig

        public string? KontrollerenDescription { get; set; }

        public string? SaksBehandlerDescription { get; set; }

        public List<Coordinate> Coordinates { get; set; }

        public SakModel()
        {
            Coordinates = new List<Coordinate>();
        }

        public string? UserId { get; set; }  // Fremmednøkkel (Foreign key)

        [ForeignKey("UserId")]
        public ApplicationUser? ApplicationUser { get; set; }  // Navigasjonsegenskap (Navigation property)

        public string? AssignedKontrollerenId { get; set; }  // Fremmednøkkel for tildelt kontroller
        [ForeignKey("AssignedKontrollerenId")]
        public ApplicationUser? AssignedKontrolleren { get; set; }  // Tildelt kontroller informasjon

        // SakStatus enum er lagt til
        public SakStatus Status { get; set; } = SakStatus.SakMottatt;

        public ArbeidStatus ArbeidStatus { get; set; } = ArbeidStatus.IkkeTilordnet;

        public KontrolStatus KontrolStatus { get; set; } = KontrolStatus.villKontrollere;
        
        // Funksjon for å sende melding når SakStatus endres
        //public void ChangeStatus(SakStatus newStatus)
        //{
        //    if (newStatus != Status) // Hvis statusen har endret seg
        //    {
        //      Status = newStatus;

        //       // Hent ny melding
        //       string message = SakStatusHelper.GetMessage(newStatus);

        //        // Legg til tilbakemelding
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
