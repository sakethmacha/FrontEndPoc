using MovieBooking.Web.Enums;
namespace MovieBooking.Web.ApiContracts.Admin
{
    public class SeatRowContract
    {
        public string SeatRow { get; set; }
        public int SeatCount { get; set; }
        public SeatType SeatType { get; set; }
        public decimal PriceMultiplier { get; set; }
    }
}
