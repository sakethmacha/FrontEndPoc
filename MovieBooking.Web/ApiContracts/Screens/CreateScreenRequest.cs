namespace MovieBooking.Web.ApiContracts.Screens
{
    public class CreateScreenRequest
    {
        public Guid TheatreId { get; set; }
        public string ScreenName { get; set; }
        public string SeatLayoutType { get; set; }
    }
}
