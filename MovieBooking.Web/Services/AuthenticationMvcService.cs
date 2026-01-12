using MovieBooking.Web.Interfaces;
using MovieBooking.Web.ViewModels;
namespace MovieBooking.Web.Services
{
    public class AuthenticationMvcService : IAuthenticationMvcService
    {
        private readonly HttpClient _httpClient;

        public AuthenticationMvcService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AuthenticationViewModel?> LoginAsync(LoginViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/authentication/login", model);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<AuthenticationViewModel>();
        }

        public async Task<bool> RegisterAsync(RegisterViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/authentication/register", model);

            return response.IsSuccessStatusCode;
        }
    }

}
