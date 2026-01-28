using MovieBooking.Web.Enums;
namespace MovieBooking.Web.ViewModels.Admin
{
    public class SeatRowViewModel
    {
        public string SeatRow { get; set; }
        public int SeatCount { get; set; }
        public SeatType SeatType { get; set; }
        public decimal PriceMultiplier { get; set; }
    }
}
