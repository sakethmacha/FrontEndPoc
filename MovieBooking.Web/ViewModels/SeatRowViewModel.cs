namespace MovieBooking.Web.ViewModels
{
    public class SeatRowViewModel
    {
        public string SeatRow { get; set; } = string.Empty;
        public int SeatCount { get; set; }
        public string SeatType { get; set; }
        public decimal PriceMultiplier { get; set; }
    }
}
