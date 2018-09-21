using System;
using System.ComponentModel.DataAnnotations;

public class FutureDateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        // You first may want to unbox "value" here and cast to to a DateTime variable!
        DateTime date = Convert.ToDateTime(value);
        if(date < DateTime.Now) {
            return new ValidationResult("Invalid date, no past dates allowed.");
        }
        return ValidationResult.Success;
    }
}