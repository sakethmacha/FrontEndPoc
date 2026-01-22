// MovieBooking.Web/Interfaces/IBookingMvcService.cs
using MovieBooking.Web.ApiContracts.Booking;
using MovieBooking.Web.ViewModels.Booking;

namespace MovieBooking.Web.Interfaces
{
    public interface IBookingMvcService
    {
        // Browse Movies & Shows
        Task<List<MovieListResponse>> GetActiveMoviesAsync();
        Task<List<TheatreShowResponse>> GetShowTimesByMovieAsync(Guid movieId, DateOnly date);

        // Seat Selection
        Task<SeatLayoutResponse> GetSeatLayoutAsync(Guid showTimeId);
        Task<LockSeatsResultViewModel> LockSeatsAsync(LockSeatsViewModel model);

        // Booking & Payment
        Task<BookingConfirmationResponse> CreateBookingAsync(CreateBookingViewModel model);
        Task<PaymentResultResponse> ProcessPaymentAsync(ProcessPaymentViewModel model);

        // User Bookings
        Task<List<UserBookingResponse>> GetMyBookingsAsync();
        Task<BookingConfirmationResponse> GetBookingDetailsAsync(Guid bookingId);
        Task CancelBookingAsync(Guid bookingId, string? reason);

        // Helper Methods
        Task<List<ScreenResponse>> GetScreensByTheatreAsync(Guid theatreId);
    }
}