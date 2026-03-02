using MovieBooking.Web.ApiContracts.Languages;
using MovieBooking.Web.ApiContracts.Movies;
using MovieBooking.Web.ApiContracts.ShowTimes;
using MovieBooking.Web.ApiContracts.Theatres;
using MovieBooking.Web.Interfaces;
using MovieBooking.Web.ViewModels;
using System.Net.Http.Headers;

namespace MovieBooking.Web.Services
{
    /// <summary>
    /// MVC service for showtime operations - calls api/ShowTime endpoints
    /// </summary>
    public class ShowTimeMvcService : IShowTimeMvcService
    {
        private readonly HttpClient HttpClient;
        private readonly IHttpContextAccessor HttpContextAccessor;

        public ShowTimeMvcService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            HttpClient = httpClient;
            HttpContextAccessor = httpContextAccessor;
        }

        private void Authenticate()
        {
            var token = HttpContextAccessor.HttpContext!
                .User.FindFirst("access_token")?.Value;
            if (string.IsNullOrEmpty(token))
                throw new UnauthorizedAccessException("JWT token missing");
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<ShowTimeResponse>> GetShowTimesAsync()
        {
            Authenticate();
            return await HttpClient.GetFromJsonAsync<List<ShowTimeResponse>>("api/ShowTime");
        }

        public async Task<ShowTimeResponse> GetShowTimeByIdAsync(Guid showTimeId)
        {
            Authenticate();
            return await HttpClient.GetFromJsonAsync<ShowTimeResponse>($"api/ShowTime/{showTimeId}");
        }

        public async Task<List<MovieShowTimeViewModel>> GetMovieWiseShowTimesAsync()
        {
            Authenticate();
            var showTimes = await HttpClient.GetFromJsonAsync<List<ShowTimeResponse>>("api/ShowTime");
            return showTimes!
                .GroupBy(x => x.MovieTitle)
                .Select(g => new MovieShowTimeViewModel
                {
                    MovieTitle = g.Key,
                    ShowTimes = g.ToList()
                }).ToList();
        }

        public async Task<AddShowTimeViewModel> GetAddShowTimeFormAsync()
        {
            Authenticate();
            return new AddShowTimeViewModel
            {
                Movies = await HttpClient.GetFromJsonAsync<List<MovieResponse>>("api/Movie"),
                Theatres = await HttpClient.GetFromJsonAsync<List<TheatreResponse>>("api/Theatre"),
                Languages = await HttpClient.GetFromJsonAsync<List<LanguageResponse>>("api/Language")
            };
        }

        public async Task<AddShowTimeBulkViewModel> GetAddShowTimeBulkFormAsync()
        {
            Authenticate();
            return new AddShowTimeBulkViewModel
            {
                Movies = await HttpClient.GetFromJsonAsync<List<MovieResponse>>("api/Movie"),
                Theatres = await HttpClient.GetFromJsonAsync<List<TheatreResponse>>("api/Theatre"),
                Languages = await HttpClient.GetFromJsonAsync<List<LanguageResponse>>("api/Language"),
                ShowTimes = new List<ShowTimeItemVm>()
            };
        }

        public async Task AddShowTimeAsync(AddShowTimeViewModel vm)
        {
            Authenticate();
            var response = await HttpClient.PostAsJsonAsync("api/ShowTime",
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
                throw new Exception($"AddShowTime failed: {(int)response.StatusCode} - {error}");
            }
        }

        public async Task AddShowTimesBulkAsync(AddShowTimeBulkViewModel vm)
        {
            Authenticate();
            foreach (var item in vm.ShowTimes)
            {
                var response = await HttpClient.PostAsJsonAsync("api/ShowTime",
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

        public async Task UpdateShowTimeAsync(Guid showTimeId, AddShowTimeViewModel vm)
        {
            Authenticate();
            var response = await HttpClient.PutAsJsonAsync($"api/ShowTime/{showTimeId}",
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
                throw new Exception($"UpdateShowTime failed: {(int)response.StatusCode} - {error}");
            }
        }

        public async Task DeleteShowTimeAsync(Guid showTimeId)
        {
            Authenticate();
            var response = await HttpClient.DeleteAsync($"api/ShowTime/{showTimeId}");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"DeleteShowTime failed: {(int)response.StatusCode} - {error}");
            }
        }
    }
}