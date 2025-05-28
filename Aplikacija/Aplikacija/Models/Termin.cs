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
        [ValidateDate]
        [DisplayName("Datum:")]
        [DataType(DataType.Date)]
        public required DateOnly Datum { get; set; }

        [Required]
        [DisplayName("Vrijeme:")]
        [DataType(DataType.Time)]
        public required TimeOnly Vrijeme { get; set; }

        [Required]
        [EnumDataType(typeof(VrstaTreninga))]
        [DisplayName("Vrsta treninga:")]
        public required VrstaTreninga Vrsta { get; set; }


        public int TrenerId { get; set; }
        [ForeignKey("TrenerId")]
        public required Korisnik Trener { get; set; }


        public bool ProvjeraTermina(DateOnly datum, TimeOnly vrijeme, VrstaTreninga vrsta)
        {
            return Datum == datum && Vrijeme == vrijeme && Vrsta == vrsta;
        }

    }
}
