namespace MovieBooking.Web.ApiContracts.Seat
{
    public class CreateSeatRowRequest
    {
        public string SeatRow { get; set; } = string.Empty;
        public int SeatCount { get; set; }
        public string SeatType { get; set; } = string.Empty;
        public decimal PriceMultiplier { get; set; }
    }
}
