namespace MovieBooking.Web.ApiContracts.Booking
{
    public class LockedSeatResponse
    {
        public Guid SeatId { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
