using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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

        [DisplayName("Pregledano:")]
        public bool Pregledano { get; set; } = false;

        [Required]
        public string KorisnikId { get; set; }

        [ForeignKey("KorisnikId")]
        [ValidateNever] 
        public Korisnik Korisnik { get; set; }
    }
}
