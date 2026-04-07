using MovieBooking.Validations.Validations;
using System.ComponentModel.DataAnnotations;

namespace MovieBooking.Web.ViewModels
{
    public class AddTheatreViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [NameValidate]
        public string Name { get; set; }
        [Required(ErrorMessage = "Location is required")]
        [NameValidate]
        public string Location { get; set; }
        public List<TimeSlotViewModel> TimeSlots { get; set; } = new();
    }
}
