using MovieBooking.Validations.Validations;
using MovieBooking.Web.ApiContracts.Theatres;
using MovieBooking.Web.Enums;
using System.ComponentModel.DataAnnotations;

namespace MovieBooking.Web.ViewModels
{
    public class AddScreenViewModel
    {
        public Guid TheatreId { get; set; }

        [Required(ErrorMessage = "ScreenName is required")]
        [NameValidate]
        public string ScreenName { get; set; }
        public SeatLayoutType SeatLayoutType { get; set; }
        public List<TheatreResponse> Theatres { get; set; } = new();

        public List<SeatRowViewModel> SeatRows { get; set; } = new();
    }
}
