namespace MovieBooking.Web.ViewModels.Admin
{
    public class RequestTheatreViewModel
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public List<TimeSlotViewModel> TimeSlots { get; set; } = new();
    }
}
