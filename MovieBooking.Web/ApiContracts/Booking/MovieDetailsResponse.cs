namespace MovieBooking.Web.ApiContracts.Booking
{
    public class MovieDetailsResponse
    {
        public string Title { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
    }
}
