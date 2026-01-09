using MovieBooking.Web.ViewModels;

namespace MovieBooking.Web.Services
{
    public class AuthenticationApiService
    {
        private readonly HttpClient HttpClient;

        public AuthenticationApiService(HttpClient HttpClient)
        {
            this.HttpClient = HttpClient;
        }

        public async Task<AuthenticationViewModel?> LoginAsync(LoginViewModel Model)
        {
            var response = await HttpClient.PostAsJsonAsync("api/authentication/login", Model);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new ApplicationException($"Login failed: {error}");
            }
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<AuthenticationViewModel>()
                : null;
        }

        public async Task<AuthenticationViewModel?> RegisterAsync(RegisterViewModel Model)
        {
            var Response = await HttpClient.PostAsJsonAsync("api/authentication/register", Model);
            return Response.IsSuccessStatusCode
                ? await Response.Content.ReadFromJsonAsync<AuthenticationViewModel>()
                : null;
        }
    }

}
