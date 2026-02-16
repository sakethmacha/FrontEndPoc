using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MovieBooking.Validations.Validations
{
    public class PasswordValidate : ValidationAttribute, IClientModelValidator
    {
        private const string Pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$";

        public PasswordValidate()
        {
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character (@$!%*?&#)";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return ValidationResult.Success;

            var password = value.ToString()!;

            if (password.Length < 8)
                return new ValidationResult("Password must be at least 8 characters long");

            if (!Regex.IsMatch(password, Pattern))
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-passwordvalidate", ErrorMessage ?? "Invalid password format");
            context.Attributes.Add("data-val-passwordvalidate-pattern", Pattern);
        }
    }
}
