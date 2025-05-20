using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    public class Termin
    {
        [Key]
        public int IdTermin { get; set; }

        [Required]
        public required DateOnly Datum { get; set; }
        [Required]
        public required TimeOnly Vrijeme { get; set; }
        [Required]
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
