namespace MovieBooking.Web.ViewModels
{
    public class AddShowTimeBulkViewModel
    {
        public Guid MovieId { get; set; }
        public Guid LanguageId { get; set; }

        public List<ShowTimeItemVm> ShowTimes { get; set; } = new();
    }
}
