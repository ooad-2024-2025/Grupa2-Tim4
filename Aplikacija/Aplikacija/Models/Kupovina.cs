﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    public class Kupovina
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdKupovina { get; set; }

        [Required]
        [DisplayName("Datum kupovine:")]
        [DataType(DataType.Date)]
        public DateOnly DatumKupovine { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Naziv artikla:")]
        public required string Artikal { get; set; }

        [Required]
        [Range(0.1, 10000.0, ErrorMessage = "Cijena mora biti između 0.1 i 10,000!")]
        [DisplayName("Cijena artikla:")]
        public required float Cijena { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Broj računa:")]
        public required string Racun { get; set; }



        [Required]
        public int IdKorisnik { get; set; }
        [ForeignKey("IdKorisnik")]
        public required Korisnik Korisnik { get; set; }



        void Kupi(string artikal, float cijena, string racun)
        {
            Artikal = artikal;
            Cijena = cijena;
            Racun = racun;
            DatumKupovine = DateOnly.FromDateTime(DateTime.Now);
        }


    }
}
