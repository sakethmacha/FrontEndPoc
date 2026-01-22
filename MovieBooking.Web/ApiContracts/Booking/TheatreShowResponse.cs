namespace MovieBooking.Web.ApiContracts.Booking
{
    public class TheatreShowResponse
    {
        public Guid TheatreId { get; set; }
        public string TheatreName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public List<ShowResponse> Shows { get; set; } = new();
    }
}
