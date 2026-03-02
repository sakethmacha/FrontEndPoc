using MovieBooking.Web.ApiContracts.Movies;
using MovieBooking.Web.Interfaces;
using MovieBooking.Web.ViewModels;
using System.Net.Http.Headers;

namespace MovieBooking.Web.Services
{
    /// <summary>
    /// MVC service for movie operations - calls api/Movie endpoints
    /// </summary>
    public class MovieMvcService : IMovieMvcService
    {
        private readonly HttpClient HttpClient;
        private readonly IHttpContextAccessor HttpContextAccessor;

        public MovieMvcService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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

        public async Task<List<MovieResponse>> GetMoviesAsync()
        {
            Authenticate();
            return await HttpClient.GetFromJsonAsync<List<MovieResponse>>("api/Movie");
        }

        public async Task<MovieResponse> GetMovieByIdAsync(Guid movieId)
        {
            Authenticate();
            return await HttpClient.GetFromJsonAsync<MovieResponse>($"api/Movie/{movieId}");
        }

        public async Task AddMovieAsync(AddMovieViewModel vm)
        {
            Authenticate();
            var response = await HttpClient.PostAsJsonAsync("api/Movie",
                new AddMovieRequest
                {
                    Title = vm.Title,
                    Description = vm.Description,
                    DurationMinutes = vm.DurationMinutes,
                    ReleaseDate = vm.ReleaseDate,
                    PosterUrl = vm.PosterUrl
                });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"AddMovie failed: {(int)response.StatusCode} - {error}");
            }
        }

        public async Task UpdateMovieAsync(Guid movieId, AddMovieViewModel vm)
        {
            Authenticate();
            var response = await HttpClient.PutAsJsonAsync($"api/Movie/{movieId}",
                new AddMovieRequest
                {
                    Title = vm.Title,
                    Description = vm.Description,
                    DurationMinutes = vm.DurationMinutes,
                    ReleaseDate = vm.ReleaseDate,
                    PosterUrl = vm.PosterUrl
                });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"UpdateMovie failed: {(int)response.StatusCode} - {error}");
            }
        }

        public async Task DeleteMovieAsync(Guid movieId)
        {
            Authenticate();
            var response = await HttpClient.DeleteAsync($"api/Movie/{movieId}");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"DeleteMovie failed: {(int)response.StatusCode} - {error}");
            }
        }
    }
}