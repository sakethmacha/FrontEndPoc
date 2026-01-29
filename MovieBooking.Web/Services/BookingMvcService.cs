
using MovieBooking.Web.ApiContracts.Booking;
using MovieBooking.Web.Interfaces;
using MovieBooking.Web.ViewModels.Booking;
using System.Net.Http.Headers;

namespace MovieBooking.Web.Services
{
    public class BookingMvcService : IBookingMvcService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookingMvcService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private void Authenticate()
        {
            var token = _httpContextAccessor.HttpContext!
                .User
                .FindFirst("access_token")?
                .Value;

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // ========== BROWSE MOVIES & SHOWS ==========

        public async Task<List<MovieListResponse>> GetActiveMoviesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<MovieListResponse>>(
                "api/bookings/movies");

            return response ?? new List<MovieListResponse>();
        }

        public async Task<List<TheatreShowResponse>> GetShowTimesByMovieAsync(Guid movieId, DateOnly date)
        {
            var response = await _httpClient.GetFromJsonAsync<List<TheatreShowResponse>>(
                $"api/bookings/movies/{movieId}/showtimes?date={date:yyyy-MM-dd}");

            return response ?? new List<TheatreShowResponse>();
        }

        // ========== SEAT SELECTION ==========

        public async Task<SeatLayoutResponse> GetSeatLayoutAsync(Guid showTimeId)
        {
            Authenticate();

            var response = await _httpClient.GetFromJsonAsync<SeatLayoutResponse>(
                $"api/bookings/showtimes/{showTimeId}/seats");

            return response!;
        }

        public async Task<LockSeatsResultViewModel> LockSeatsAsync(LockSeatsViewModel model)
        {
            Authenticate();

            var request = new LockSeatsRequest
            {
                ShowTimeId = model.ShowTimeId,
                SeatIds = model.SeatIds
            };

            var response = await _httpClient.PostAsJsonAsync(
                "api/bookings/lock-seats", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to lock seats: {error}");
            }

            var result = await response.Content.ReadFromJsonAsync<LockSeatsResultResponse>();

            return new LockSeatsResultViewModel
            {
                Success = result!.Success,
                Message = result.Message,
                ExpiresAt = result.ExpiresAt,
                LockedSeats = result.LockedSeats.Select(ls => new LockedSeatViewModel
                {
                    SeatId = ls.SeatId,
                    SeatNumber = ls.SeatNumber,
                    Price = ls.Price
                }).ToList()
            };
        }

        // ========== BOOKING & PAYMENT ==========

        public async Task<BookingConfirmationResponse> CreateBookingAsync(CreateBookingViewModel model)
        {
            Authenticate();

            var request = new CreateBookingRequest
            {
                ShowTimeId = model.ShowTimeId,
                SeatIds = model.SeatIds
            };

            var response = await _httpClient.PostAsJsonAsync(
                "api/bookings", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to create booking: {error}");
            }

            return await response.Content.ReadFromJsonAsync<BookingConfirmationResponse>()!;
        }

        public async Task<PaymentResultResponse> ProcessPaymentAsync(ProcessPaymentViewModel model)
        {
            Authenticate();

            var request = new ProcessPaymentRequest
            {
                BookingId = model.BookingId,
                PaymentMethod = model.PaymentMethod,
                PaymentGateway = model.PaymentGateway
            };

            var response = await _httpClient.PostAsJsonAsync(
                "api/bookings/payment", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Payment failed: {error}");
            }

            return await response.Content.ReadFromJsonAsync<PaymentResultResponse>()!;
        }

        // ========== USER BOOKINGS ==========

        public async Task<List<UserBookingResponse>> GetMyBookingsAsync()
        {
            Authenticate();

            var response = await _httpClient.GetFromJsonAsync<List<UserBookingResponse>>(
                "api/bookings/my-bookings");

            return response ?? new List<UserBookingResponse>();
        }

        public async Task<BookingConfirmationResponse> GetBookingDetailsAsync(Guid bookingId)
        {
            Authenticate();

            var response = await _httpClient.GetFromJsonAsync<BookingConfirmationResponse>(
                $"api/bookings/{bookingId}");

            return response!;
        }

        public async Task CancelBookingAsync(Guid bookingId, string? reason)
        {
            Authenticate();

            var request = new CancelBookingRequest
            {
                BookingId = bookingId,
                Reason = reason ?? string.Empty
            };

            var response = await _httpClient.PostAsJsonAsync(
                "api/book/cancel", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to cancel booking: {error}");
            }
        }

        // ========== HELPER METHODS ==========

        public async Task<List<ScreenResponse>> GetScreensByTheatreAsync(Guid theatreId)
        {
            Authenticate();

            var response = await _httpClient.GetFromJsonAsync<List<ScreenResponse>>(
                $"api/superadmin/screens/by-theatre/{theatreId}");

            return response ?? new List<ScreenResponse>();
        }
    }
}