using MovieBooking.Validations.Validations;
using System.ComponentModel.DataAnnotations;

namespace MovieBooking.Web.ViewModels
{
    public class AddMovieViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [NameValidate]
        public string Title { get; set; }
        [NameValidate]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        [NameValidate]
        [Required(ErrorMessage = "DurationMinutes is required")]
        public int DurationMinutes { get; set; }
        [Required(ErrorMessage = "ReleaseDate is required")]
        public DateTime ReleaseDate { get; set; }
        //public string PosterUrl { get; set; }

        public IFormFile? PosterFile { get; set; }
    }
}
