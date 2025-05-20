using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    public class Trening
    {
        [Key]
        public int IdTrening { get; set; }

        [Required]
        public required DateTime Datum { get; set; }
        [Required]
        public required TimeSpan Vrijeme { get; set; }
        [Required]
        public required VrstaTreninga Tip { get; set; }


        public int ClanId { get; set; }
        public required Korisnik Clan { get; set; }

        public int TrenerId { get; set; }
        public required Korisnik Trener { get; set; }


    }
}
