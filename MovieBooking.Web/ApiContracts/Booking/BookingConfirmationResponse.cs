using MovieBooking.Web.Enums;
namespace MovieBooking.Web.ApiContracts.Booking
{
    public class BookingConfirmationResponse
    {
        public Guid BookingId { get; set; }
        public string BookingReference { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public MovieDetailsResponse Movie { get; set; } = null!;
        public TheatreDetailsResponse Theatre { get; set; } = null!;
        public List<BookedSeatResponse> Seats { get; set; } = new();
    }
}
