using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models
{
    public class PrijavaZaZaposljavanje
    {
        [Key]
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public string CV { get; set; } 
        public bool Pregledano { get; set; } = false; 
    }
}
