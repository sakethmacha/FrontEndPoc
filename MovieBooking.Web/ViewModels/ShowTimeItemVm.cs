namespace MovieBooking.Web.ViewModels
{
    public class ShowTimeItemVm
    {
        public Guid TheatreId { get; set; }
        public Guid ScreenId { get; set; }
        public decimal BasePrice { get; set; }
        public DateOnly ShowDate { get; set; }
    }
}
