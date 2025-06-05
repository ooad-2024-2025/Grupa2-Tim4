using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    public class PrijavaZaZaposljavanje
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPrijava { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Ime:")]
        public required string Ime { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Prezime:")]
        public required string Prezime { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email adresa:")]
        public required string Email { get; set; }

        [Required]
        [StringLength(500)]
        [DisplayName("CV:")]
        public required string CV { get; set; }

        [Required]
        [DisplayName("Pregledano:")]
        public bool Pregledano { get; set; } = false;


        public int KorisnikId { get; set; }
        [ForeignKey("KorisnikId")]
        public required Korisnik Korisnik { get; set; }


    }
}
