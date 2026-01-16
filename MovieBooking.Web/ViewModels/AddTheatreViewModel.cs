using MovieBooking.Web.ApiContracts.Theatres;

namespace MovieBooking.Web.ViewModels
{
    public class AddTheatreViewModel
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public List<TimeSlotViewModel> TimeSlots { get; set; } = new();
    }
}
