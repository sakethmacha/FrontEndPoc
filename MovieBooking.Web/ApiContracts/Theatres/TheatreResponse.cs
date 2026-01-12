namespace MovieBooking.Web.ApiContracts.Theatres
{
    public class TheatreResponse
    {
        public Guid TheatreId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public bool IsActive { get; set; }
        public string ApprovalStatus { get; set; }
    }
}
