namespace MovieBooking.Web.ViewModels.Booking
{
    public class ProcessPaymentViewModel
    {
        public Guid BookingId { get; set; }
        public string PaymentMethod { get; set; } = "CARD";
        public string PaymentGateway { get; set; } = "Razorpay";
    }
}
