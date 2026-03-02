using MovieBooking.Web.ApiContracts.Seat;
using MovieBooking.Web.Enums;
namespace MovieBooking.Web.ApiContracts.Screens
{
    public class CreateScreenRequest
    {
        public Guid TheatreId { get; set; }
        public string ScreenName { get; set; }
        public SeatLayoutType SeatLayoutType { get; set; }

        //  NEW
        public List<CreateSeatRowRequest> SeatRows { get; set; } = new();
    }
}
