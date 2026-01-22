namespace MovieBooking.Web.ApiContracts.Booking
{
    public class SeatRowResponse
    {
        public string RowName { get; set; } = string.Empty;
        public List<SeatResponse> Seats { get; set; } = new();
    }
}
