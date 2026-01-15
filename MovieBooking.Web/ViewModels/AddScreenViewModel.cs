using MovieBooking.Web.ApiContracts.Theatres;

namespace MovieBooking.Web.ViewModels
{
    public class AddScreenViewModel
    {
        public Guid TheatreId { get; set; }
        public string ScreenName { get; set; }
        public string SeatLayoutType { get; set; }
        public List<TheatreResponse> Theatres { get; set; } = new();

        public List<SeatRowViewModel> SeatRows { get; set; } = new();
    }
}
