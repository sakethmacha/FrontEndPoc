using MovieBooking.Web.ApiContracts.AdminRequests;
using MovieBooking.Web.ApiContracts.Languages;
using MovieBooking.Web.ApiContracts.Movies;
using MovieBooking.Web.ApiContracts.Screens;
using MovieBooking.Web.ApiContracts.Seat;
using MovieBooking.Web.ApiContracts.ShowTimes;
using MovieBooking.Web.ApiContracts.Theatres;
using MovieBooking.Web.Interfaces;
using MovieBooking.Web.ViewModels;
using System.Net.Http.Headers;

namespace MovieBooking.Web.Services
{
    public class SuperAdminMvcService : ISuperAdminMvcService
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _ctx;

        public SuperAdminMvcService(HttpClient http, IHttpContextAccessor ctx)
        {
            _http = http;
            _ctx = ctx;
        }

        private void Auth()
        {
            var token = _ctx.HttpContext!
                .User
                .FindFirst("access_token")?
                .Value;

            if (string.IsNullOrEmpty(token))
                throw new UnauthorizedAccessException("JWT token missing");

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        // ========== EXISTING METHODS ==========

        public async Task<List<MovieResponse>> GetMoviesAsync()
        {
            Auth();
            return await _http.GetFromJsonAsync<List<MovieResponse>>("api/superadmin/movies");
        }

        public async Task AddMovieAsync(AddMovieViewModel vm)
        {
            Auth();

            var response = await _http.PostAsJsonAsync(
                "api/superadmin/movies",
                new AddMovieRequest
                {
                    Title = vm.Title,
                    Description = vm.Description,
                    DurationMinutes = vm.DurationMinutes,
                    ReleaseDate = vm.ReleaseDate,
                    PosterUrl = vm.PosterUrl
                });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"AddMovie failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task<List<TheatreResponse>> GetTheatresAsync()
        {
            Auth();
            return await _http.GetFromJsonAsync<List<TheatreResponse>>("api/superadmin/theatres");
        }

        public async Task<List<ScreenResponse>> GetScreensByTheatreAsync(Guid theatreId)
        {
            Auth();
            return await _http.GetFromJsonAsync<List<ScreenResponse>>(
                $"api/superadmin/screens/by-theatre/{theatreId}");
        }

        public async Task AddTheatreAsync(AddTheatreViewModel vm)
        {
            Auth();

            var response = await _http.PostAsJsonAsync(
                "api/superadmin/theatres",
                new CreateTheatreRequest
                {
                    Name = vm.Name,
                    Location = vm.Location,
                    TimeSlots = vm.TimeSlots.Select(ts => new TimeSlotRequest
                    {
                        StartTime = ts.StartTime,
                        EndTime = ts.EndTime
                    }).ToList()
                });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"AddTheatre failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task AddScreenAsync(AddScreenViewModel vm)
        {
            Auth();

            var response = await _http.PostAsJsonAsync(
                "api/superadmin/screens",
                new CreateScreenRequest
                {
                    TheatreId = vm.TheatreId,
                    ScreenName = vm.ScreenName,
                    SeatLayoutType = vm.SeatLayoutType,
                    SeatRows = vm.SeatRows.Select(r => new CreateSeatRowRequest
                    {
                        SeatRow = r.SeatRow,
                        SeatCount = r.SeatCount,
                        SeatType = r.SeatType,
                        PriceMultiplier = r.PriceMultiplier
                    }).ToList()
                });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"AddScreen failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task<List<ScreenResponse>> GetScreensAsync()
        {
            Auth();
            return await _http.GetFromJsonAsync<List<ScreenResponse>>("api/superadmin/screens");
        }

        public async Task<List<ShowTimeResponse>> GetShowTimesAsync()
        {
            Auth();
            return await _http.GetFromJsonAsync<List<ShowTimeResponse>>("api/superadmin/showtimes");
        }

        public async Task<AddShowTimeViewModel> GetAddShowTimeFormAsync()
        {
            Auth();
            return new AddShowTimeViewModel
            {
                Movies = await _http.GetFromJsonAsync<List<MovieResponse>>("api/superadmin/movies"),
                Theatres = await _http.GetFromJsonAsync<List<TheatreResponse>>("api/superadmin/theatres"),
                Languages = await _http.GetFromJsonAsync<List<LanguageResponse>>("api/superadmin/languages")
            };
        }

        public async Task<AddShowTimeBulkViewModel> GetAddShowTimeBulkFormAsync()
        {
            Auth();

            return new AddShowTimeBulkViewModel
            {
                Movies = await _http.GetFromJsonAsync<List<MovieResponse>>("api/superadmin/movies"),
                Theatres = await _http.GetFromJsonAsync<List<TheatreResponse>>("api/superadmin/theatres"),
                Languages = await _http.GetFromJsonAsync<List<LanguageResponse>>("api/superadmin/languages"),
                ShowTimes = new List<ShowTimeItemVm>()
            };
        }

        public async Task AddShowTimeAsync(AddShowTimeViewModel vm)
        {
            Auth();

            var response = await _http.PostAsJsonAsync(
                "api/superadmin/showtimes",
                new CreateShowTimeRequest
                {
                    MovieId = vm.MovieId,
                    TheatreId = vm.TheatreId,
                    ScreenId = vm.ScreenId,
                    LanguageId = vm.LanguageId,
                    ShowDate = vm.ShowDate,
                    BasePrice = vm.BasePrice
                });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"AddShowTime failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task AddShowTimesBulkAsync(AddShowTimeBulkViewModel vm)
        {
            Auth();

            foreach (var item in vm.ShowTimes)
            {
                var response = await _http.PostAsJsonAsync(
                    "api/superadmin/showtimes",
                    new CreateShowTimeRequest
                    {
                        TheatreId = item.TheatreId,
                        ScreenId = item.ScreenId,
                        MovieId = vm.MovieId,
                        LanguageId = vm.LanguageId,
                        ShowDate = item.ShowDate,
                        BasePrice = item.BasePrice
                    });

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Bulk AddShowTime failed: {error}");
                }
            }
        }

        public async Task<List<AdminRequestResponse>> GetRequestsAsync()
        {
            Auth();
            return await _http.GetFromJsonAsync<List<AdminRequestResponse>>("api/superadmin/requests");
        }

        public async Task ApproveRequestAsync(Guid id)
        {
            Auth();
            await _http.PutAsync($"api/superadmin/requests/{id}/approve", null);
        }

        public async Task RejectRequestAsync(Guid id)
        {
            Auth();
            await _http.PutAsync($"api/superadmin/requests/{id}/reject", null);
        }

        public async Task<List<LanguageResponse>> GetLanguagesAsync()
        {
            Auth();
            return await _http.GetFromJsonAsync<List<LanguageResponse>>("api/superadmin/languages");
        }

        // ========== NEW: GET BY ID METHODS ==========

        public async Task<MovieResponse> GetMovieByIdAsync(Guid movieId)
        {
            Auth();
            return await _http.GetFromJsonAsync<MovieResponse>($"api/superadmin/movies/{movieId}");
        }

        public async Task<TheatreResponse> GetTheatreByIdAsync(Guid theatreId)
        {
            Auth();
            return await _http.GetFromJsonAsync<TheatreResponse>($"api/superadmin/theatres/{theatreId}");
        }

        public async Task<CreateScreenRequest> GetScreenByIdAsync(Guid screenId)
        {
            Auth();
            return await _http.GetFromJsonAsync<CreateScreenRequest>($"api/superadmin/screens/{screenId}");
        }

        public async Task<ShowTimeResponse> GetShowTimeByIdAsync(Guid showTimeId)
        {
            Auth();
            return await _http.GetFromJsonAsync<ShowTimeResponse>($"api/superadmin/showtimes/{showTimeId}");
        }

        public async Task<LanguageResponse> GetLanguageByIdAsync(Guid languageId)
        {
            Auth();
            return await _http.GetFromJsonAsync<LanguageResponse>($"api/superadmin/languages/{languageId}");
        }

        // ========== NEW: UPDATE METHODS ==========

        public async Task UpdateMovieAsync(Guid movieId, AddMovieViewModel vm)
        {
            Auth();

            var response = await _http.PutAsJsonAsync(
                $"api/superadmin/movies/{movieId}",
                new AddMovieRequest
                {
                    Title = vm.Title,
                    Description = vm.Description,
                    DurationMinutes = vm.DurationMinutes,
                    ReleaseDate = vm.ReleaseDate,
                    PosterUrl = vm.PosterUrl
                });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"UpdateMovie failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task UpdateTheatreAsync(Guid theatreId, AddTheatreViewModel vm)
        {
            Auth();

            var response = await _http.PutAsJsonAsync(
                $"api/superadmin/theatres/{theatreId}",
                new CreateTheatreRequest
                {
                    Name = vm.Name,
                    Location = vm.Location,
                    TimeSlots = vm.TimeSlots.Select(ts => new TimeSlotRequest
                    {
                        StartTime = ts.StartTime,
                        EndTime = ts.EndTime
                    }).ToList()
                });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"UpdateTheatre failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task UpdateScreenAsync(Guid screenId, AddScreenViewModel vm)
        {
            Auth();

            var response = await _http.PutAsJsonAsync(
                $"api/superadmin/screens/{screenId}",
                new CreateScreenRequest
                {
                    TheatreId = vm.TheatreId,
                    ScreenName = vm.ScreenName,
                    SeatLayoutType = vm.SeatLayoutType,
                    SeatRows = vm.SeatRows.Select(r => new CreateSeatRowRequest
                    {
                        SeatRow = r.SeatRow,
                        SeatCount = r.SeatCount,
                        SeatType = r.SeatType,
                        PriceMultiplier = r.PriceMultiplier
                    }).ToList()
                });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"UpdateScreen failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task UpdateShowTimeAsync(Guid showTimeId, AddShowTimeViewModel vm)
        {
            Auth();

            var response = await _http.PutAsJsonAsync(
                $"api/superadmin/showtimes/{showTimeId}",
                new CreateShowTimeRequest
                {
                    MovieId = vm.MovieId,
                    TheatreId = vm.TheatreId,
                    ScreenId = vm.ScreenId,
                    LanguageId = vm.LanguageId,
                    ShowDate = vm.ShowDate,
                    BasePrice = vm.BasePrice
                });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"UpdateShowTime failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task UpdateLanguageAsync(Guid languageId, string name)
        {
            Auth();

            var response = await _http.PutAsJsonAsync(
                $"api/superadmin/languages/{languageId}",
                new { Name = name });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"UpdateLanguage failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        // ========== NEW: DELETE METHODS ==========

        public async Task DeleteMovieAsync(Guid movieId)
        {
            Auth();

            var response = await _http.DeleteAsync($"api/superadmin/movies/{movieId}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"DeleteMovie failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task DeleteTheatreAsync(Guid theatreId)
        {
            Auth();

            var response = await _http.DeleteAsync($"api/superadmin/theatres/{theatreId}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"DeleteTheatre failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task DeleteScreenAsync(Guid screenId)
        {
            Auth();

            var response = await _http.DeleteAsync($"api/superadmin/screens/{screenId}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"DeleteScreen failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task DeleteShowTimeAsync(Guid showTimeId)
        {
            Auth();

            var response = await _http.DeleteAsync($"api/superadmin/showtimes/{showTimeId}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"DeleteShowTime failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task DeleteLanguageAsync(Guid languageId)
        {
            Auth();

            var response = await _http.DeleteAsync($"api/superadmin/languages/{languageId}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"DeleteLanguage failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task AddLanguageAsync(string name)
        {
            Auth();

            var response = await _http.PostAsJsonAsync(
                "api/superadmin/languages",
                new { Name = name });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"AddLanguage failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }
    }
}