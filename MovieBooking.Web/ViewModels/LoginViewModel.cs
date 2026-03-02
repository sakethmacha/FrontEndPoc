using System.ComponentModel.DataAnnotations;
using MovieBooking.Validations.Validations;
namespace MovieBooking.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        [EmailValidate]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
        [PasswordValidate]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;
    }

}
