namespace MovieBooking.Web.ApiContracts.Booking
{
    public class BookedSeatResponse
    {
        public string SeatNumber { get; set; } = string.Empty;
        public string SeatType { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
