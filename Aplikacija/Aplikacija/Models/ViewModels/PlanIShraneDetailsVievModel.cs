using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models.ViewModels
{
    public class PlanIshraneDetailsViewModel
    {
        public PlanIshrane Plan { get; set; }
        public double BMI { get; set; }
        public int PotrebneKalorije { get; set; }
        public string NacinVjezbanja { get; set; }
    }
}
