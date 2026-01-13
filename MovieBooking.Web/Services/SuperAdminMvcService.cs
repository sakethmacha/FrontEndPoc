using MovieBooking.Web.ApiContracts.AdminRequests;
using MovieBooking.Web.ApiContracts.Languages;
using MovieBooking.Web.ApiContracts.Movies;
using MovieBooking.Web.ApiContracts.Screens;
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


        public async Task<List<MovieResponse>> GetMoviesAsync()
        {
            Auth();
            return await _http.GetFromJsonAsync<List<MovieResponse>>("api/superadmin/movies");
        }

        public async Task AddMovieAsync(AddMovieViewModel vm)
        {
            Auth(); // MUST attach a VALID JWT

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

            //  THIS IS THE MOST IMPORTANT PART
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

        //public async Task<List<ScreenResponse>> GetScreensByTheatreAsync()
        //{
        //        Auth();
        //     return await _http.GetFromJsonAsync<List<ScreenResponse>>("api/superadmin/screens/by-theatre/{theatreId}");

        //}
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
                    Location = vm.Location
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
                    SeatLayoutType = vm.SeatLayoutType
                });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"AddScreen failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
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
                    StartTime = vm.StartTime,
                    EndTime = vm.StartTime.AddHours(3),
                    BasePrice = vm.BasePrice
                });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"AddShowTime failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
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
    }

}
