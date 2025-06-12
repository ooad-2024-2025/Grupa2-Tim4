using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Aplikacija.Models.ViewModels;

namespace Aplikacija.Models
{
  

    public class PlanIshrane
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPlanishrane { get; set; }

        [Required(ErrorMessage = "Cilj plana je obavezan.")]
        [EnumDataType(typeof(TipCilja))]
        [DisplayName("Cilj plana:")]
        public TipCilja Ciljevi { get; set; }

       
        public string Plan { get; set; } = string.Empty;

        [Required(ErrorMessage = "Datum generisanja je obavezan.")]
        [DisplayName("Datum generisanja:")]
        [DataType(DataType.Date)]
        public DateTime DatumGenerisanja { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Kilaža je obavezna.")]
        [Range(30, 300, ErrorMessage = "Kilaža mora biti između 30 i 300 kg.")]
        [DisplayName("Kilaža (kg):")]
        public double Kilaza { get; set; }

        [Required(ErrorMessage = "Godine su obavezne.")]
        [Range(10, 100, ErrorMessage = "Godine moraju biti između 10 i 100.")]
        [DisplayName("Godine:")]
        public int Godine { get; set; }

        [Required(ErrorMessage = "Visina je obavezna.")]
        [Range(100, 250, ErrorMessage = "Visina mora biti između 100 i 250 cm.")]
        [DisplayName("Visina (cm):")]
        public int Visina { get; set; }

        // Foreign Key
        public string ClanId { get; set; } = string.Empty;

        // Navigation Property
        [ForeignKey("ClanId")]
        public virtual Korisnik? Clan { get; set; }
    }
}