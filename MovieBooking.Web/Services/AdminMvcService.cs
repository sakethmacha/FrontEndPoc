using MovieBooking.Web.ApiContracts.Admin;
using MovieBooking.Web.Interfaces;
using MovieBooking.Web.ViewModels.Admin;
using System.Net.Http.Headers;

namespace MovieBooking.Web.Services
{
    public class AdminMvcService : IAdminMvcService
    {
        private readonly HttpClient HttpClient;
        private readonly IHttpContextAccessor HttpContextAccessor;

        public AdminMvcService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            HttpClient = httpClient;
            HttpContextAccessor = httpContextAccessor;
        }

        private void Auth()
        {
            var token = HttpContextAccessor.HttpContext!
                .User
                .FindFirst("access_token")?
                .Value;

            if (string.IsNullOrEmpty(token))
                throw new UnauthorizedAccessException("JWT token missing");

            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        // ========== THEATRE REQUESTS ==========

        public async Task RequestTheatreAsync(RequestTheatreViewModel requestTheatreViewModel)
        {
            Auth();

            var request = new CreateTheatreRequestContract
            {
                Name = requestTheatreViewModel.Name,
                Location = requestTheatreViewModel.Location,
                TimeSlots = requestTheatreViewModel.TimeSlots.Select(ts => new TimeSlotContract
                {
                    StartTime = ts.StartTime,
                    EndTime = ts.EndTime
                }).ToList()
            };

            var response = await HttpClient.PostAsJsonAsync("api/admin/theatres/request", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Request Theatre failed: {(int)response.StatusCode} - {error}");
            }
        }

        public async Task<List<TheatreRequestResponse>> GetTheatreRequestsAsync()
        {
            Auth();
            return await HttpClient.GetFromJsonAsync<List<TheatreRequestResponse>>("api/admin/theatres/requests")
                   ?? new List<TheatreRequestResponse>();
        }

        public async Task<List<TheatreRequestResponse>> GetApprovedTheatresAsync()
        {
            Auth();
            return await HttpClient.GetFromJsonAsync<List<TheatreRequestResponse>>("api/admin/theatres/approved")
                   ?? new List<TheatreRequestResponse>();
        }

        // ========== SCREEN REQUESTS ==========

        public async Task RequestScreenAsync(RequestScreenViewModel requestScreenViewModel)
        {
            Auth();

            var request = new CreateScreenRequestContract
            {
                TheatreId = requestScreenViewModel.TheatreId,
                ScreenName = requestScreenViewModel.ScreenName,
                SeatLayoutType = requestScreenViewModel.SeatLayoutType,
                SeatRows = requestScreenViewModel.SeatRows.Select(sr => new SeatRowContract
                {
                    SeatRow = sr.SeatRow,
                    SeatCount = sr.SeatCount,
                    SeatType = sr.SeatType,
                    PriceMultiplier = sr.PriceMultiplier
                }).ToList()
            };

            var response = await HttpClient.PostAsJsonAsync("api/admin/screens/request", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Request Screen failed: {(int)response.StatusCode} - {error}");
            }
        }

        public async Task<List<ScreenRequestResponse>> GetScreenRequestsAsync()
        {
            Auth();
            return await HttpClient.GetFromJsonAsync<List<ScreenRequestResponse>>("api/admin/screens/requests")
                   ?? new List<ScreenRequestResponse>();
        }

        public async Task<List<ScreenRequestResponse>> GetApprovedScreensAsync()
        {
            Auth();
            return await HttpClient.GetFromJsonAsync<List<ScreenRequestResponse>>("api/admin/screens/approved")
                   ?? new List<ScreenRequestResponse>();
        }

        // ========== HELPER METHODS ==========

        public async Task<List<TheatreDropdownItem>> GetTheatresForScreenAsync()
        {
            Auth();
            var theatres = await HttpClient.GetFromJsonAsync<List<TheatreRequestResponse>>("api/admin/theatres/for-screen")
                          ?? new List<TheatreRequestResponse>();

            return theatres.Select(t => new TheatreDropdownItem
            {
                TheatreId = t.TheatreId,
                Name = t.Name,
                Location = t.Location
            }).ToList();
        }
    }
}