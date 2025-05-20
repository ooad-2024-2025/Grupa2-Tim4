using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models
{
    public class Kupovina
    {
        [Key]
        public int Id { get; set; }
        public DateOnly DatumKupovine { get; set; }
        public string Artikal { get; set; } 
        public float Cijena { get; set; }   
        public string Racun { get; set; }


        void Kupi(string artikal, float cijena, string racun)
        {
            Artikal = artikal;
            Cijena = cijena;
            Racun = racun;
            DatumKupovine = DateOnly.FromDateTime(DateTime.Now);
        }


    }
}
