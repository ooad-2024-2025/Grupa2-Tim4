using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models
{
    public enum VrstaTreninga
    {
        [Display(Name = "Individualni")]
        Individualni,
        [Display(Name = "Grupni")]
        Grupni
    }
}
