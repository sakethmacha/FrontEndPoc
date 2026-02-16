using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MovieBooking.Validations.Validations
{
    public class EmailValidate : ValidationAttribute, IClientModelValidator
    {
        // Strict pattern: requires @ and proper domain with TLD (com, org, etc.)
        private const string Pattern = @"^[a-zA-Z0-9]+([._-][a-zA-Z0-9]+)*@[a-zA-Z0-9]+([.-][a-zA-Z0-9]+)*\.[a-zA-Z]{2,}$";

        public EmailValidate()
        {
            ErrorMessage = "Please enter a valid email address (e.g., user@example.com)";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return ValidationResult.Success;

            var email = value.ToString()!.Trim();

            // Check if email contains @
            if (!email.Contains("@"))
                return new ValidationResult("Email must contain @ symbol");

            // Check if email contains . after @
            var atIndex = email.IndexOf("@");
            var dotIndex = email.LastIndexOf(".");

            if (dotIndex <= atIndex)
                return new ValidationResult("Email must contain a domain with extension (e.g., .com, .org)");

            // Check minimum length after last dot (TLD should be at least 2 chars)
            var tld = email.Substring(dotIndex + 1);
            if (tld.Length < 2)
                return new ValidationResult("Email domain extension must be at least 2 characters");

            // Validate with regex pattern
            if (!Regex.IsMatch(email, Pattern))
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-emailvalidate", ErrorMessage ?? "Invalid email format");
            context.Attributes.Add("data-val-emailvalidate-pattern", Pattern);
        }
    }
}

