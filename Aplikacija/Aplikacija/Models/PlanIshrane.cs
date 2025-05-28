using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    public class PlanIshrane
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPlanishrane { get; set; }

        [Required]
        [EnumDataType(typeof(TipCilja))]
        [DisplayName("Cilj plana:")]
        public required TipCilja Ciljevi { get; set; }

        [Required]
        [StringLength(500)]
        [DisplayName("Plan ishrane:")]
        public required string Plan { get; set; }

        [Required]
        [DisplayName("Datum generisanja:")]
        [DataType(DataType.Date)]
        public required DateTime DatumGenerisanja { get; set; }

        [Required]
        [Range(30, 300)]
        [DisplayName("Kilaža (kg):")]
        public required double Kilaza { get; set; }

        [Required]
        [Range(10, 100)]
        [DisplayName("Godine:")]
        public required int Godine { get; set; }


        public int ClanId { get; set; }
        [ForeignKey("ClanId")]
        public required Korisnik Clan { get; set; }
    }
}
