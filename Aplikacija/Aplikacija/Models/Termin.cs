using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    public class Termin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTermin { get; set; }

        [Required]
        [DisplayName("Datum:")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Datum { get; set; }

        [Required]
        [DisplayName("Vrijeme:")]
        [DataType(DataType.Time)]
        public required TimeOnly Vrijeme { get; set; }

        [Required]
        [EnumDataType(typeof(VrstaTreninga))]
        [DisplayName("Vrsta treninga:")]
        public required VrstaTreninga Vrsta { get; set; }

        [BindNever]
        public string TrenerId { get; set; } = string.Empty;

        [ForeignKey("TrenerId")]
        [BindNever]
        public Korisnik? Trener { get; set; }

        // Navigation properties
        public ICollection<PrijavaNaTermin> Prijave { get; set; } = new List<PrijavaNaTermin>();

        // NOVO - veza sa realizovanim trenizima
        public ICollection<Trening> RealizovaniTreninzi { get; set; } = new List<Trening>();

        public bool ProvjeraTermina(DateTime datum, TimeOnly vrijeme, VrstaTreninga vrsta)
        {
            return Datum.Date == datum.Date
                && Vrijeme == vrijeme
                && Vrsta == vrsta;
        }
    }
}