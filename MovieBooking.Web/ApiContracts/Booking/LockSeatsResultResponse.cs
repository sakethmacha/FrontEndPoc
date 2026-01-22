namespace MovieBooking.Web.ApiContracts.Booking
{
    public class LockSeatsResultResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime? ExpiresAt { get; set; }
        public List<LockedSeatResponse> LockedSeats { get; set; } = new();
    }

}
