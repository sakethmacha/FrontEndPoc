using MovieBooking.Web.Enums;
namespace MovieBooking.Web.ApiContracts.Booking
{
    public class SeatResponse
    {
        public Guid SeatId { get; set; }
        public string SeatRow { get; set; } = string.Empty;
        public int SeatColumn { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public SeatType SeatType { get; set; } 
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsLocked { get; set; }
    }
}
