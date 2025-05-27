using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models
{
    public enum TipKorisnika
    {
        [Display(Name = "Administrator")]
        Administrator,
        [Display(Name = "Član")]
        Clan,
        [Display(Name = "Recepcioner")]
        Recepcioner,
        [Display(Name = "Trener")]
        Trener
    }
}
