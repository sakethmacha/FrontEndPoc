namespace MovieBooking.Web.ApiContracts.Booking
{
    public class CreateBookingRequest
    {
        public Guid ShowTimeId { get; set; }
        public List<Guid> SeatIds { get; set; } = new();
    }
}
