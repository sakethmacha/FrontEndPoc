namespace MovieBooking.Web.ViewModels.Admin
{
    public class RequestScreenViewModel
    {
        public Guid TheatreId { get; set; }
        public string ScreenName { get; set; }
        public string SeatLayoutType { get; set; }
        public List<SeatRowViewModel> SeatRows { get; set; } = new();

        // For dropdown
        public List<TheatreDropdownItem> AvailableTheatres { get; set; } = new();
    }
}
