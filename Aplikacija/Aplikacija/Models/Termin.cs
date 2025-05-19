using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models
{
    public class Termin
    {
        [Key]
        public int Id { get; set; }
        public DateOnly Datum { get; set; }
        public TimeOnly Vrijeme { get; set; }
        public VrstaTreninga Vrsta { get; set; }
        public Korisnik Trener { get; set; }

        public bool ProvjeraTermina(DateOnly datum, TimeOnly vrijeme, VrstaTreninga vrsta)
        {
            return Datum == datum && Vrijeme == vrijeme && Vrsta == vrsta;
        }

    }
}
