using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models
{
    public enum StatusTreninga
    {
        [Display(Name = "Planiran")]
        Planiran,

        [Display(Name = "U toku")]
        UToku,

        [Display(Name = "Završen")]
        Zavrsen,

        [Display(Name = "Otkazan")]
        Otkazan
    }
}