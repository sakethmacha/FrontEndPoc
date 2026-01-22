
using MovieBooking.Web.ApiContracts.Booking;

namespace MovieBooking.Web.ViewModels.Booking
{
    public class SelectShowViewModel
    {
        public Guid MovieId { get; set; }
        public MovieListResponse? Movie { get; set; }
        public DateOnly SelectedDate { get; set; }
        public List<TheatreShowResponse> Theatres { get; set; } = new();
    }
}