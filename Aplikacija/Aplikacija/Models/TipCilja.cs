using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models
{
    public enum TipCilja
    {
        [Display(Name = "Gubitak kilaže")]
        GubitakKilaze,
        [Display(Name = "Pocećanje mišićne mase")]
        PovecanjeMisicneMase,
        [Display(Name = "Pocećanje izdržljjivosti")]
        PovecanjeIzdrzljivosti
    }
}
