namespace MovieBooking.Web.ApiContracts.Admin
{
    public class CreateTheatreRequestContract
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public List<TimeSlotContract> TimeSlots { get; set; } = new();
    }

}
