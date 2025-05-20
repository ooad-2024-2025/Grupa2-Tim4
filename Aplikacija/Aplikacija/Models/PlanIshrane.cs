using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    public class PlanIshrane
    {
        [Key]
        public int IdPlanishrane { get; set; }

        [Required]
        public required TipCilja Ciljevi { get; set; }
        [Required]
        public required string Plan { get; set; }
        [Required]
        public required DateTime DatumGenerisanja { get; set; }
        [Required]
        public required double Kilaza { get; set; }
        [Required]
        public required int Godine { get; set; }


        public int ClanId { get; set; }
        [ForeignKey("ClanId")]
        public required Korisnik Clan { get; set; }
    }
}
