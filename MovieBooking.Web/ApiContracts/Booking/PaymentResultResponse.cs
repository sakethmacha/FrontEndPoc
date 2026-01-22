namespace MovieBooking.Web.ApiContracts.Booking
{
    public class PaymentResultResponse
    {
        public Guid PaymentId { get; set; }
        public string TransactionId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime? PaidAt { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
