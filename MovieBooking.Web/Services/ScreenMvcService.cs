using MovieBooking.Web.ApiContracts.Screens;
using MovieBooking.Web.ApiContracts.Seat;
using MovieBooking.Web.Interfaces;
using MovieBooking.Web.ViewModels;
using System.Net.Http.Headers;

namespace MovieBooking.Web.Services
{
    /// <summary>
    /// MVC service for screen operations - calls api/Screen endpoints
    /// </summary>
    public class ScreenMvcService : IScreenMvcService
    {
        private readonly HttpClient HttpClient;
        private readonly IHttpContextAccessor HttpContextAccessor;

        public ScreenMvcService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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

        public async Task<List<ScreenResponse>> GetScreensAsync()
        {
            Authenticate();
            return await HttpClient.GetFromJsonAsync<List<ScreenResponse>>("api/Screen");
        }

        public async Task<CreateScreenRequest> GetScreenByIdAsync(Guid screenId)
        {
            Authenticate();
            return await HttpClient.GetFromJsonAsync<CreateScreenRequest>($"api/Screen/{screenId}");
        }

        public async Task<List<ScreenResponse>> GetScreensByTheatreAsync(Guid theatreId)
        {
            Authenticate();
            return await HttpClient.GetFromJsonAsync<List<ScreenResponse>>(
                $"api/Screen/by-theatre/{theatreId}");
        }

        public async Task AddScreenAsync(AddScreenViewModel vm)
        {
            Authenticate();
            var response = await HttpClient.PostAsJsonAsync("api/Screen",
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
                throw new Exception($"AddScreen failed: {(int)response.StatusCode} - {error}");
            }
        }

        public async Task UpdateScreenAsync(Guid screenId, AddScreenViewModel vm)
        {
            Authenticate();
            var response = await HttpClient.PutAsJsonAsync($"api/Screen/{screenId}",
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
                throw new Exception($"UpdateScreen failed: {(int)response.StatusCode} - {error}");
            }
        }

        public async Task DeleteScreenAsync(Guid screenId)
        {
            Authenticate();
            var response = await HttpClient.DeleteAsync($"api/Screen/{screenId}");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"DeleteScreen failed: {(int)response.StatusCode} - {error}");
            }
        }
    }
}