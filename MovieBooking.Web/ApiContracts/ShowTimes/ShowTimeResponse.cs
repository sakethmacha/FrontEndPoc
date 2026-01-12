namespace MovieBooking.Web.ApiContracts.ShowTimes
{
    public class ShowTimeResponse
    {
        public Guid ShowTimeId { get; set; }
        public string MovieTitle { get; set; }
        public string TheatreName { get; set; }
        public string ScreenName { get; set; }
        public string Language { get; set; }
        public DateTime StartTime { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsActive { get; set; }
    }
}
