
using MovieBooking.Web.Enums;
namespace MovieBooking.Web.ApiContracts.Booking
{
    public class BookedSeatResponse
    {
        public string SeatNumber { get; set; } = string.Empty;
        public string SeatType { get; set; } 
        public decimal Price { get; set; }
    }
}
