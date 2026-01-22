namespace MovieBooking.Web.ApiContracts.Booking
{
    public class CancelBookingRequest
    {
        public Guid BookingId { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
