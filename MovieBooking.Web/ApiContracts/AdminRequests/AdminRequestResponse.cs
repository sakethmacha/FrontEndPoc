using MovieBooking.Web.Enums;
namespace MovieBooking.Web.ApiContracts.AdminRequests
{
    public class AdminRequestResponse
    {
        public Guid AdminRequestId { get; set; }
        public string RequestType { get; set; }
        public ApprovalStatus Status { get; set; }
        public DateTime RequestedAt { get; set; }

        public string RequestedBy { get; set; }

        //public string TheatreName { get; set; }
        //public string ScreenName { get; set; }
    }

}
