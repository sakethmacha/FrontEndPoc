namespace MovieBooking.Web.ApiContracts.AdminRequests
{
    public class AdminRequestResponse
    {
        public Guid AdminRequestId { get; set; }
        public string RequestType { get; set; }
        public string Status { get; set; }
        public DateTime RequestedAt { get; set; }
    }

}
