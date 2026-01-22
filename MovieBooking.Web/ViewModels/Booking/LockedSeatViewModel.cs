namespace MovieBooking.Web.ViewModels.Booking
{
    public class LockedSeatViewModel
    {
        public Guid SeatId { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
