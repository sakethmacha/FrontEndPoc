namespace MovieBooking.Web.ApiContracts.Admin
{
    public class TheatreRequestResponse
    {
        public Guid TheatreId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string ApprovalStatus { get; set; }
        public DateTime RequestedAt { get; set; }
        public List<TimeSlotContract> TimeSlots { get; set; } = new();
    }
}
