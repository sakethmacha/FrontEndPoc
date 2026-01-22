namespace MovieBooking.Web.ViewModels.Booking
{
    public class CreateBookingViewModel
    {
        public Guid ShowTimeId { get; set; }
        public List<Guid> SeatIds { get; set; } = new();
    }
}
