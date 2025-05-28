using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    public class Korisnik
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdKorisnik { get; set; }

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

        [Required]
        [StringLength(20, MinimumLength = 4)]
        [DisplayName("Korisničko ime:")]
        public required string Username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [DisplayName("Lozinka:")]
        public required string Password { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email adresa:")]
        public required string Email { get; set; }

        [Required]
        [EnumDataType(typeof(TipKorisnika))]
        [DisplayName("Tip korisnika:")]
        public required TipKorisnika Tip { get; set; }



        public virtual required ICollection<Kupovina> Kupovine { get; set; }
        public virtual required ICollection<PrijavaZaZaposljavanje> Prijave { get; set; }
        public virtual required ICollection<Termin> TerminiTrenera { get; set; }
        public virtual required PlanIshrane Plan_Ishrane { get; set; }
        public virtual required ICollection<Trening> TreninziKaoClan { get; set; }
        public virtual required ICollection<Trening> TreninziKaoTrener { get; set; }

    }

}
