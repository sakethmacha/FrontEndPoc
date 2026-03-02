using MovieBooking.Web.ApiContracts.Languages;
using MovieBooking.Web.Interfaces;
using System.Net.Http.Headers;

namespace MovieBooking.Web.Services
{
    /// <summary>
    /// MVC service for language operations - calls api/Language endpoints
    /// </summary>
    public class LanguageMvcService : ILanguageMvcService
    {
        private readonly HttpClient HttpClient;
        private readonly IHttpContextAccessor HttpContextAccessor;

        public LanguageMvcService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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

        public async Task<List<LanguageResponse>> GetLanguagesAsync()
        {
            Authenticate();
            return await HttpClient.GetFromJsonAsync<List<LanguageResponse>>("api/Language");
        }

        public async Task<LanguageResponse> GetLanguageByIdAsync(Guid languageId)
        {
            Authenticate();
            return await HttpClient.GetFromJsonAsync<LanguageResponse>($"api/Language/{languageId}");
        }

        public async Task AddLanguageAsync(string name)
        {
            Authenticate();
            var response = await HttpClient.PostAsJsonAsync("api/Language", new { Name = name });
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"AddLanguage failed: {(int)response.StatusCode} - {error}");
            }
        }

        public async Task UpdateLanguageAsync(Guid languageId, string name)
        {
            Authenticate();
            var response = await HttpClient.PutAsJsonAsync($"api/Language/{languageId}", new { Name = name });
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"UpdateLanguage failed: {(int)response.StatusCode} - {error}");
            }
        }

        public async Task DeleteLanguageAsync(Guid languageId)
        {
            Authenticate();
            var response = await HttpClient.DeleteAsync($"api/Language/{languageId}");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"DeleteLanguage failed: {(int)response.StatusCode} - {error}");
            }
        }
    }
}