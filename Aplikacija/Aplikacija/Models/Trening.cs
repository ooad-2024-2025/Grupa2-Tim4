using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    public class Trening
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTrening { get; set; }

        [Required]
        [ValidateDate]
        [DisplayName("Datum treninga:")]
        [DataType(DataType.Date)]
        public required DateTime Datum { get; set; }

        [Required]
        [DisplayName("Vrijeme treninga:")]
        [DataType(DataType.Time)]
        public required TimeSpan Vrijeme { get; set; }

        [Required]
        [EnumDataType(typeof(VrstaTreninga))]
        [DisplayName("Vrsta treninga:")]
        public required VrstaTreninga Tip { get; set; }


        public int ClanId { get; set; }
        public required Korisnik Clan { get; set; }

        public int TrenerId { get; set; }
        public required Korisnik Trener { get; set; }


    }
}
