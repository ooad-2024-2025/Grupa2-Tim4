using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    public class Vezba
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdVezba { get; set; }

        [Required]
        [ForeignKey("Trening")]
        public int TreningId { get; set; }

        [Required]
        [DisplayName("Naziv vežbe:")]
        [StringLength(100, ErrorMessage = "Naziv vežbe ne može biti duži od 100 karaktera.")]
        public string NazivVezbe { get; set; } = string.Empty;

        [Required]
        [DisplayName("Broj serija:")]
        [Range(1, 20, ErrorMessage = "Broj serija mora biti između 1 i 20.")]
        public int Serije { get; set; }

        [Required]
        [DisplayName("Broj ponavljanja:")]
        [Range(1, 100, ErrorMessage = "Broj ponavljanja mora biti između 1 i 100.")]
        public int Ponavljanja { get; set; }

        [DisplayName("Težina (kg):")]
        [Range(0.5, 500, ErrorMessage = "Težina mora biti između 0.5 i 500 kg.")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Tezina { get; set; }

        [DisplayName("Trajanje:")]
        [DataType(DataType.Time)]
        public TimeSpan? Trajanje { get; set; }

        [DisplayName("Napomene:")]
        [StringLength(500, ErrorMessage = "Napomene ne mogu biti duže od 500 karaktera.")]
        public string? Napomene { get; set; }

        [Required]
        [DisplayName("Redosled:")]
        [Range(1, 50, ErrorMessage = "Redosled mora biti između 1 i 50.")]
        public int Redosled { get; set; }

        // Navigation property
        public Trening? Trening { get; set; }
    }
}