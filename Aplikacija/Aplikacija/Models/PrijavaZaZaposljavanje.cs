using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    public class PrijavaZaZaposljavanje
    {
        [Key]
        public int IdPrijava { get; set; }

        [Required]
        public required string Ime { get; set; }
        [Required]
        public required string Prezime { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string CV { get; set; } 
        [Required]
        public required bool Pregledano { get; set; } = false;


        public int KorisnikId { get; set; }
        [ForeignKey("KorisnikId")]
        public required Korisnik Korisnik { get; set; }


    }
}
