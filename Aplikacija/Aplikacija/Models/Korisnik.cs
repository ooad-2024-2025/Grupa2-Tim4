using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    public class Korisnik : IdentityUser
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Dozvoljena su samo slova i razmak!")]
        [DisplayName("Ime korisnika:")]
        public required string Ime { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Dozvoljena su samo slova i razmak!")]
        [DisplayName("Prezime korisnika:")]
        public required string Prezime { get; set; }


        [EnumDataType(typeof(TipKorisnika))]
        [DisplayName("Tip korisnika:")]
        public TipKorisnika Tip { get; set; }


        public virtual ICollection<Kupovina>? Kupovine { get; set; }
        public virtual ICollection<PrijavaZaZaposljavanje>? Prijave { get; set; }
        public virtual ICollection<Termin>? TerminiTrenera { get; set; }
        public virtual PlanIshrane? Plan_Ishrane { get; set; }
        public virtual ICollection<Trening>? TreninziKaoClan { get; set; }
        public virtual ICollection<Trening>? TreninziKaoTrener { get; set; }

    }

}
