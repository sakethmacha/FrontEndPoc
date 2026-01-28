using MovieBooking.Web.ApiContracts.Admin;
using MovieBooking.Web.Interfaces;
using MovieBooking.Web.ViewModels.Admin;
using System.Net.Http.Headers;

namespace MovieBooking.Web.Services
{
    public class AdminMvcService : IAdminMvcService
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _ctx;

        public AdminMvcService(HttpClient http, IHttpContextAccessor ctx)
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

        // ========== THEATRE REQUESTS ==========

        public async Task RequestTheatreAsync(RequestTheatreViewModel vm)
        {
            Auth();

            var request = new CreateTheatreRequestContract
            {
                Name = vm.Name,
                Location = vm.Location,
                TimeSlots = vm.TimeSlots.Select(ts => new TimeSlotContract
                {
                    StartTime = ts.StartTime,
                    EndTime = ts.EndTime
                }).ToList()
            };

            var response = await _http.PostAsJsonAsync("api/admin/theatres/request", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Request Theatre failed: {(int)response.StatusCode} - {error}");
            }
        }

        public async Task<List<TheatreRequestResponse>> GetMyTheatreRequestsAsync()
        {
            Auth();
            return await _http.GetFromJsonAsync<List<TheatreRequestResponse>>("api/admin/theatres/requests")
                   ?? new List<TheatreRequestResponse>();
        }

        public async Task<List<TheatreRequestResponse>> GetMyApprovedTheatresAsync()
        {
            Auth();
            return await _http.GetFromJsonAsync<List<TheatreRequestResponse>>("api/admin/theatres/approved")
                   ?? new List<TheatreRequestResponse>();
        }

        // ========== SCREEN REQUESTS ==========

        public async Task RequestScreenAsync(RequestScreenViewModel vm)
        {
            Auth();

            var request = new CreateScreenRequestContract
            {
                TheatreId = vm.TheatreId,
                ScreenName = vm.ScreenName,
                SeatLayoutType = vm.SeatLayoutType,
                SeatRows = vm.SeatRows.Select(sr => new SeatRowContract
                {
                    SeatRow = sr.SeatRow,
                    SeatCount = sr.SeatCount,
                    SeatType = sr.SeatType,
                    PriceMultiplier = sr.PriceMultiplier
                }).ToList()
            };

            var response = await _http.PostAsJsonAsync("api/admin/screens/request", request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Request Screen failed: {(int)response.StatusCode} - {error}");
            }
        }

        public async Task<List<ScreenRequestResponse>> GetMyScreenRequestsAsync()
        {
            Auth();
            return await _http.GetFromJsonAsync<List<ScreenRequestResponse>>("api/admin/screens/requests")
                   ?? new List<ScreenRequestResponse>();
        }

        public async Task<List<ScreenRequestResponse>> GetMyApprovedScreensAsync()
        {
            Auth();
            return await _http.GetFromJsonAsync<List<ScreenRequestResponse>>("api/admin/screens/approved")
                   ?? new List<ScreenRequestResponse>();
        }

        // ========== HELPER METHODS ==========

        public async Task<List<TheatreDropdownItem>> GetMyTheatresForScreenAsync()
        {
            Auth();
            var theatres = await _http.GetFromJsonAsync<List<TheatreRequestResponse>>("api/admin/theatres/for-screen")
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