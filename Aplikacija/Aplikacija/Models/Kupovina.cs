using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    public class Kupovina
    {
        [Key]
        public int IdKupovina { get; set; }

        [Required]
        public DateOnly DatumKupovine { get; set; }
        [Required]
        public required string Artikal { get; set; }
        [Required]
        public required float Cijena { get; set; }   
        [Required]
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
