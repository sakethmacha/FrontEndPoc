namespace MovieBooking.Web.ApiContracts.Theatres
{
    public class TimeSlotRequest
    {
        public string StartTime { get; set; } = string.Empty; // "06:30"
        public string EndTime { get; set; } = string.Empty;   // "09:30"
    }
}
