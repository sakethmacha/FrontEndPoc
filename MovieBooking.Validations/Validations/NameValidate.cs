using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
namespace MovieBooking.Validations.Validations
{
    public class NameValidate : ValidationAttribute, IClientModelValidator
    {
        private const string Pattern = @"^[a-zA-Z\s]+$";

        public NameValidate()
        {
            ErrorMessage = "Name can only contain letters and spaces";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return ValidationResult.Success;

            var name = value.ToString()!;

            if (!Regex.IsMatch(name, Pattern))
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-namevalidate", ErrorMessage ?? "Invalid name format");
            context.Attributes.Add("data-val-namevalidate-pattern", Pattern);
        }
    }
}
