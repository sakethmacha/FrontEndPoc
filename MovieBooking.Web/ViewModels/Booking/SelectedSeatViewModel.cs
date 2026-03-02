using MovieBooking.Web.Enums;
namespace MovieBooking.Web.ViewModels.Booking
{
    public class SelectedSeatViewModel
    {
        public Guid SeatId { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public SeatType SeatType { get; set; } 
        public decimal Price { get; set; }
    }
}
