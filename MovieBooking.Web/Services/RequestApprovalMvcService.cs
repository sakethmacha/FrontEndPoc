using MovieBooking.Web.ApiContracts.AdminRequests;
using MovieBooking.Web.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MovieBooking.Web.Services
{
    /// <summary>
    /// MVC service for request approval operations - calls api/RequestApproval endpoints
    /// </summary>
    public class RequestApprovalMvcService : IRequestApprovalMvcService
    {
        private readonly HttpClient HttpClient;
        private readonly IHttpContextAccessor HttpContextAccessor;

        public RequestApprovalMvcService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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

        public async Task<List<AdminRequestResponse>> GetRequestsAsync()
        {
            Authenticate();
            return await HttpClient.GetFromJsonAsync<List<AdminRequestResponse>>("api/RequestApproval");
        }
        //public async Task<List<AdminRequestResponse>> GetRequestsAsync()
        //{
        //    Authenticate();

        //    var response = await HttpClient.GetAsync("api/RequestApproval");

        //    var json = await response.Content.ReadAsStringAsync();

        //    Console.WriteLine(json); // 👈 VERY IMPORTANT

        //    var options = new JsonSerializerOptions
        //    {
        //        PropertyNameCaseInsensitive = true
        //    };

        //    return JsonSerializer.Deserialize<List<AdminRequestResponse>>(json, options);
        //}
        public async Task ApproveRequestAsync(Guid id)
        {
            Authenticate();
            await HttpClient.PutAsync($"api/RequestApproval/{id}/approve", null);
        }

        public async Task RejectRequestAsync(Guid id)
        {
            Authenticate();
            await HttpClient.PutAsync($"api/RequestApproval/{id}/reject", null);
        }
    }
}