using System.ComponentModel.DataAnnotations;

namespace Aplikacija.Models
{
    public class ValidateDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dateValue)
            {
                if (dateValue > DateTime.Now)
                    return ValidationResult.Success!;
                else
                    return new ValidationResult("Datum mora biti u budućnosti (ne smije biti današnji ili prošli)!");
            }

            return new ValidationResult("Neispravan format datuma!");
        }
    }
}
