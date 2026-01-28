namespace MovieBooking.Web.ApiContracts.Admin
{
    public class CreateScreenRequestContract
    {
        public Guid TheatreId { get; set; }
        public string ScreenName { get; set; }
        public string SeatLayoutType { get; set; }
        public List<SeatRowContract> SeatRows { get; set; } = new();
    }
}
