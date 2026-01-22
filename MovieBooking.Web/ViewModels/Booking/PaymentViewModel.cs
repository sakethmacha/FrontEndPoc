namespace MovieBooking.Web.ViewModels.Booking
{
    public class PaymentViewModel
    {
        public Guid BookingId { get; set; }
        public string BookingReference { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public string TheatreName { get; set; } = string.Empty;
        public DateTime ShowTime { get; set; }
        public string Seats { get; set; } = string.Empty;
    }
}
