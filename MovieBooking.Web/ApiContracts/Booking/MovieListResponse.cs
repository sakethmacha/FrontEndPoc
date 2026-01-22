// MovieBooking.Web/ApiContracts/Booking/MovieListResponse.cs
namespace MovieBooking.Web.ApiContracts.Booking
{
    public class MovieListResponse
    {
        public Guid MovieId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string PosterUrl { get; set; } = string.Empty;
    }
}