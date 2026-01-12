namespace MovieBooking.Web.ApiContracts.Movies
{
    public class MovieResponse
    {
        public Guid MovieId { get; set; }
        public string Title { get; set; }
        public int DurationMinutes { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsActive { get; set; }
    }
}
