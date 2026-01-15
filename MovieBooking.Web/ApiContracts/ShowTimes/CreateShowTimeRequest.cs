namespace MovieBooking.Web.ApiContracts.ShowTimes
{
    public class CreateShowTimeRequest
    {
        public Guid MovieId { get; set; }
        public Guid TheatreId { get; set; }
        public Guid ScreenId { get; set; }
        public Guid LanguageId { get; set; }
        public DateOnly ShowDate {  get; set; }
        public decimal BasePrice { get; set; }
    }
}
