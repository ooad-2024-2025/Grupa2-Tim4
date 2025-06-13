using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    public class Trening
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTrening { get; set; }

        [Required]
        [ForeignKey("Termin")]
        [DisplayName("Termin ID:")]
        public int TerminId { get; set; }

        [Required]
        [ForeignKey("Clan")]
        [DisplayName("Član ID:")]
        public string ClanId { get; set; } = string.Empty;

        [Required]
        [EnumDataType(typeof(StatusTreninga))]
        [DisplayName("Status treninga:")]
        public StatusTreninga Status { get; set; } = StatusTreninga.Planiran;

        [DisplayName("Plan treninga:")]
        [StringLength(1000, ErrorMessage = "Plan treninga ne može biti duži od 1000 karaktera.")]
        public string? PlanTreninga { get; set; }

        [DisplayName("Napomene:")]
        [StringLength(500, ErrorMessage = "Napomene ne mogu biti duže od 500 karaktera.")]
        public string? Napomene { get; set; }

        [DisplayName("Datum kreiranja:")]
        [DataType(DataType.DateTime)]
        public DateTime DatumKreiranja { get; set; } = DateTime.Now;

        [DisplayName("Poslednja izmena:")]
        [DataType(DataType.DateTime)]
        public DateTime? PoslednaIzmena { get; set; }

        // Navigation properties
        public Termin? Termin { get; set; }
        public Korisnik? Clan { get; set; }
        public ICollection<Vezba> Vezbe { get; set; } = new List<Vezba>();

        // Computed properties za lakše korišćenje
        [NotMapped]
        public DateTime Datum => Termin?.Datum ?? DateTime.MinValue;

        [NotMapped]
        public TimeOnly Vrijeme => Termin?.Vrijeme ?? TimeOnly.MinValue;

        [NotMapped]
        public VrstaTreninga Tip => Termin?.Vrsta ?? VrstaTreninga.Grupni;

        [NotMapped]
        public string? TrenerId => Termin?.TrenerId;

        [NotMapped]
        public Korisnik? Trener => Termin?.Trener;
    }
}
