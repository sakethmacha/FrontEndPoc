namespace MovieBooking.Web.ApiContracts.Booking
{
    public class ProcessPaymentRequest
    {
        public Guid BookingId { get; set; }
        public string PaymentMethod { get; set; } = "CARD"; // CARD, UPI, NETBANKING
        public string PaymentGateway { get; set; } = "Razorpay";
    }
}
