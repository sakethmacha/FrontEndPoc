namespace MovieBooking.Web.ViewModels.Booking
{
    public class ReviewBookingViewModel
    {
        public Guid ShowTimeId { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public string TheatreName { get; set; } = string.Empty;
        public string ScreenName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public List<SelectedSeatViewModel> SelectedSeats { get; set; } = new();
        public decimal TotalAmount { get; set; }
    }
}
