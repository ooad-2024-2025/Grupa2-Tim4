using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models
{
    public enum TipCilja
    {
        [Display(Name = "Gubljenje težine")]
        GubljenjeTezine,
        [Display(Name = "Održavanje težine")]
        OdrzavanjeTezine,
        [Display(Name = "Povećanje mišićne mase")]
        PovecanjeMuski,
        [Display(Name = "Povećanje energije")]
        PovecanjeEnergije,
        [Display(Name = "Opći zdravstveni cilj")]
        OpciZdravstveniCilj
    }
}
