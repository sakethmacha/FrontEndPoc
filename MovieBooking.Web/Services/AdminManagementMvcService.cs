using MovieBooking.Web.ApiContracts.Admin;
using MovieBooking.Web.Interfaces;
using MovieBooking.Web.ViewModels;
using System.Net.Http.Headers;

namespace MovieBooking.Web.Services
{
    /// <summary>
    /// MVC service for admin user operations - calls api/AdminManagement endpoints
    /// </summary>
    public class AdminManagementMvcService : IAdminManagementMvcService
    {
        private readonly HttpClient HttpClient;
        private readonly IHttpContextAccessor HttpContextAccessor;

        public AdminManagementMvcService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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

        public async Task<List<AdminDto>> GetAdminsAsync()
        {
            Authenticate();
            return await HttpClient.GetFromJsonAsync<List<AdminDto>>("api/AdminManagement");
        }

        public async Task<AdminDto> GetAdminByIdAsync(Guid adminId)
        {
            Authenticate();
            return await HttpClient.GetFromJsonAsync<AdminDto>($"api/AdminManagement/{adminId}");
        }

        public async Task CreateAdminAsync(AddAdminViewModel vm)
        {
            Authenticate();
            var response = await HttpClient.PostAsJsonAsync("api/AdminManagement",
                new CreateAdminDto
                {
                    Name = vm.Name,
                    Email = vm.Email,
                    Password = vm.Password
                });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"CreateAdmin failed: {(int)response.StatusCode} - {error}");
            }
        }

        public async Task UpdateAdminAsync(Guid adminId, AddAdminViewModel vm)
        {
            Authenticate();
            var response = await HttpClient.PutAsJsonAsync($"api/AdminManagement/{adminId}", vm);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"UpdateAdmin failed: {(int)response.StatusCode} - {error}");
            }
        }

        public async Task DeactivateAdminAsync(Guid adminId)
        {
            Authenticate();
            var response = await HttpClient.DeleteAsync($"api/AdminManagement/{adminId}");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"DeactivateAdmin failed: {(int)response.StatusCode} - {error}");
            }
        }
    }
}