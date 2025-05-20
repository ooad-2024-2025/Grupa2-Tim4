using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models
{
    public class Korisnik
    {
        [Key]
        public int IdKorisnik { get; set; }

        [Required]
        public required string Ime { get; set; }
        [Required]
        public required string Prezime { get; set; }
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public required TipKorisnika Tip { get; set; }


        public virtual required ICollection<Kupovina> Kupovine { get; set; }
        public virtual required ICollection<PrijavaZaZaposljavanje> Prijave { get; set; }
        public virtual required ICollection<Termin> TerminiTrenera { get; set; }
        public virtual required PlanIshrane Plan_Ishrane { get; set; }
        public virtual required ICollection<Trening> TreninziKaoClan { get; set; }
        public virtual required ICollection<Trening> TreninziKaoTrener { get; set; }

    }

}
