namespace MovieBooking.Web.ApiContracts.Screens
{
    public class ScreenResponse
    {
        public Guid ScreenId { get; set; }
        public string ScreenName { get; set; }
        public string SeatLayoutType { get; set; }
        public bool IsActive { get; set; }
    }
}
