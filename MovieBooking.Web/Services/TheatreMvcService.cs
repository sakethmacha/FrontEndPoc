using MovieBooking.Web.ApiContracts.Theatres;
using MovieBooking.Web.Interfaces;
using MovieBooking.Web.ViewModels;
using System.Net.Http.Headers;

namespace MovieBooking.Web.Services
{
    /// <summary>
    /// MVC service for theatre operations - calls api/Theatre endpoints
    /// </summary>
    public class TheatreMvcService : ITheatreMvcService
    {
        private readonly HttpClient HttpClient;
        private readonly IHttpContextAccessor HttpContextAccessor;

        public TheatreMvcService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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

        public async Task<List<TheatreResponse>> GetTheatresAsync()
        {
            Authenticate();
            return await HttpClient.GetFromJsonAsync<List<TheatreResponse>>("api/Theatre");
        }

        public async Task<TheatreResponse> GetTheatreByIdAsync(Guid theatreId)
        {
            Authenticate();
            return await HttpClient.GetFromJsonAsync<TheatreResponse>($"api/Theatre/{theatreId}");
        }

        public async Task AddTheatreAsync(AddTheatreViewModel vm)
        {
            Authenticate();
            var response = await HttpClient.PostAsJsonAsync("api/Theatre",
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
                throw new Exception($"AddTheatre failed: {(int)response.StatusCode} - {error}");
            }
        }

        public async Task UpdateTheatreAsync(Guid theatreId, AddTheatreViewModel vm)
        {
            Authenticate();
            var response = await HttpClient.PutAsJsonAsync($"api/Theatre/{theatreId}",
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
                throw new Exception($"UpdateTheatre failed: {(int)response.StatusCode} - {error}");
            }
        }

        public async Task DeleteTheatreAsync(Guid theatreId)
        {
            Authenticate();
            var response = await HttpClient.DeleteAsync($"api/Theatre/{theatreId}");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"DeleteTheatre failed: {(int)response.StatusCode} - {error}");
            }
        }
    }
}