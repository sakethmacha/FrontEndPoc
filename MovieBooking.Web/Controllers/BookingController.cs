// MovieBooking.Web/Controllers/BookingController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Web.Interfaces;
using MovieBooking.Web.ViewModels.Booking;

namespace MovieBooking.Web.Controllers
{
    [Authorize(Roles = "User")]
    public class BookingController : Controller
    {
        private readonly IBookingMvcService BookingService;

        public BookingController(IBookingMvcService bookingService)
        {
            BookingService = bookingService;
        }

        // ========== STEP 1: BROWSE MOVIES ==========

        /// <summary>
        /// Landing page - Show all active movies
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var movies = await BookingService.GetActiveMoviesAsync();
            return View(movies);
        }

        // ========== STEP 2: SELECT SHOW ==========

        /// <summary>
        /// Show theatres and showtimes for a movie
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> SelectShow(Guid movieId, string? date = null)
        {
            DateOnly selectedDate;

            if (string.IsNullOrEmpty(date) || !DateOnly.TryParse(date, out selectedDate))
            {
                selectedDate = DateOnly.FromDateTime(DateTime.Today);
            }

            var viewModel = new SelectShowViewModel
            {
                MovieId = movieId,
                SelectedDate = selectedDate,
                Theatres = await BookingService.GetShowTimesByMovieAsync(movieId, selectedDate)
            };

            // Get movie details for display
            var movies = await BookingService.GetActiveMoviesAsync();
            viewModel.Movie = movies.FirstOrDefault(m => m.MovieId == movieId);

            return View(viewModel);
        }

        // ========== STEP 3: SELECT SEATS ==========

        /// <summary>
        /// Show seat layout for selected show
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> SelectSeats(Guid showTimeId)
        {
            var seatLayout = await BookingService.GetSeatLayoutAsync(showTimeId);
            return View(seatLayout);
        }

        /// <summary>
        /// Lock selected seats (AJAX)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> LockSeats([FromBody] LockSeatsViewModel loclSeatsViewModel)
        {
            try
            {
                var result = await BookingService.LockSeatsAsync(loclSeatsViewModel);

                if (!result.Success)
                    return BadRequest(new { success = false, message = result.Message });

                return Ok(new
                {
                    success = true,
                    message = result.Message,
                    expiresAt = result.ExpiresAt,
                    lockedSeats = result.LockedSeats
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // ========== STEP 4: REVIEW BOOKING ==========

        /// <summary>
        /// Review booking details before payment
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ReviewBooking(Guid showTimeId, string seatIds)
        {
            var seatIdList = seatIds.Split(',')
                .Select(id => Guid.Parse(id))
                .ToList();

            var seatLayout = await BookingService.GetSeatLayoutAsync(showTimeId);

            var viewModel = new ReviewBookingViewModel
            {
                ShowTimeId = showTimeId,
                MovieTitle = seatLayout.MovieTitle,
                TheatreName = seatLayout.TheatreName,
                ScreenName = seatLayout.ScreenName,
                StartTime = seatLayout.StartTime,
                SelectedSeats = seatLayout.SeatRows
                    .SelectMany(r => r.Seats)
                    .Where(s => seatIdList.Contains(s.SeatId))
                    .Select(s => new SelectedSeatViewModel
                    {
                        SeatId = s.SeatId,
                        SeatNumber = s.SeatNumber,
                        SeatType = s.SeatType,
                        Price = s.Price
                    })
                    .ToList()
            };

            viewModel.TotalAmount = viewModel.SelectedSeats.Sum(s => s.Price);

            return View(viewModel);
        }

        // ========== STEP 5: CONFIRM BOOKING ==========

        /// <summary>
        /// Create booking (before payment)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ConfirmBooking(CreateBookingViewModel createBookingViewModel)
        {
            try
            {
                var booking = await BookingService.CreateBookingAsync(createBookingViewModel);

                return RedirectToAction("Payment", new { bookingId = booking.BookingId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("SelectSeats", new { showTimeId = createBookingViewModel.ShowTimeId });
            }
        }

        // ========== STEP 6: PAYMENT ==========

        /// <summary>
        /// Payment page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Payment(Guid bookingId)
        {
            var booking = await BookingService.GetBookingDetailsAsync(bookingId);

            var viewModel = new PaymentViewModel
            {
                BookingId = bookingId,
                BookingReference = booking.BookingReference,
                Amount = booking.TotalAmount,
                MovieTitle = booking.Movie.Title,
                TheatreName = booking.Theatre.Name,
                ShowTime = booking.Theatre.ShowTime,
                Seats = string.Join(", ", booking.Seats.Select(s => s.SeatNumber))
            };

            return View(viewModel);
        }

        /// <summary>
        /// Process payment
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ProcessPayment(ProcessPaymentViewModel processPaymentViewModel)
        {
            try
            {
                var payment = await BookingService.ProcessPaymentAsync(processPaymentViewModel);

                if (payment.Status == "SUCCESS")
                {
                    TempData["Success"] = "Payment successful! Your booking is confirmed.";
                    return RedirectToAction("BookingSuccess", new { bookingId = processPaymentViewModel.BookingId });
                }
                else
                {
                    TempData["Error"] = "Payment failed. Please try again.";
                    return RedirectToAction("Payment", new { bookingId = processPaymentViewModel.BookingId });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Payment", new { bookingId = processPaymentViewModel.BookingId });
            }
        }

        // ========== STEP 7: BOOKING SUCCESS ==========

        /// <summary>
        /// Booking confirmation page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> BookingSuccess(Guid bookingId)
        {
            var booking = await BookingService.GetBookingDetailsAsync(bookingId);
            return View(booking);
        }

        // ========== MY BOOKINGS ==========

        /// <summary>
        /// View all user bookings
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> MyBookings()
        {
            var bookings = await BookingService.GetMyBookingsAsync();
            return View(bookings);
        }

        /// <summary>
        /// View booking details
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> BookingDetails(Guid bookingId)
        {
            var booking = await BookingService.GetBookingDetailsAsync(bookingId);
            return View(booking);
        }

        // ========== CANCEL BOOKING ==========

        /// <summary>
        /// Cancel booking
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CancelBooking(Guid bookingId, string? reason = null)
        {
            try
            {
                await BookingService.CancelBookingAsync(bookingId, reason);
                TempData["Success"] = "Booking cancelled successfully. Refund will be processed shortly.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("MyBookings");
        }

        // ========== AJAX ENDPOINTS ==========

        /// <summary>
        /// Get available dates for a movie (AJAX)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAvailableDates(Guid movieId)
        {
            // Return next 7 days
            var dates = Enumerable.Range(0, 7)
                .Select(i => DateOnly.FromDateTime(DateTime.Today.AddDays(i)))
                .Select(d => new
                {
                    value = d.ToString("yyyy-MM-dd"),
                    text = d.ToString("ddd, dd MMM")
                })
                .ToList();

            return Ok(dates);
        }

        /// <summary>
        /// Get screens for a theatre (AJAX)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetScreensByTheatre(Guid theatreId)
        {
            var screens = await BookingService.GetScreensByTheatreAsync(theatreId);
            return Ok(screens);
        }
    }
}