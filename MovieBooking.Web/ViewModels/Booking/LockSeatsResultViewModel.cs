namespace MovieBooking.Web.ViewModels.Booking
{
    public class LockSeatsResultViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime? ExpiresAt { get; set; }
        public List<LockedSeatViewModel> LockedSeats { get; set; } = new();
    }

}
