using MovieBooking.Web.Enums;
namespace MovieBooking.Web.ApiContracts.Admin
{
    public class ScreenRequestResponse
    {
        public Guid ScreenId { get; set; }
        public string ScreenName { get; set; }
        public string TheatreName { get; set; }
        public string SeatLayoutType { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public DateTime RequestedAt { get; set; }
    }
}
