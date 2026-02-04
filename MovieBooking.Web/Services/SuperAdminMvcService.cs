using MovieBooking.Web.ApiContracts.Admin;
using MovieBooking.Web.ApiContracts.AdminRequests;
using MovieBooking.Web.ApiContracts.Languages;
using MovieBooking.Web.ApiContracts.Movies;
using MovieBooking.Web.ApiContracts.Screens;
using MovieBooking.Web.ApiContracts.Seat;
using MovieBooking.Web.ApiContracts.ShowTimes;
using MovieBooking.Web.ApiContracts.Theatres;
using MovieBooking.Web.Interfaces;
using MovieBooking.Web.ViewModels;
using System.Net.Http.Headers;

namespace MovieBooking.Web.Services
{
    public class SuperAdminMvcService : ISuperAdminMvcService
    {
        private readonly HttpClient HttpClient;
        private readonly IHttpContextAccessor HttpContextAccessor;

        public SuperAdminMvcService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            HttpClient = httpClient;
            HttpContextAccessor = httpContextAccessor;
        }

        private void Authentication()
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

        // ========== EXISTING METHODS ==========
        public async Task CreateAdminAsync(AddAdminViewModel addAdminViewModel)
        {
            Authentication();
            var dto = new CreateAdminDto
            {
                Name = addAdminViewModel.Name,
                Email = addAdminViewModel.Email,
                Password = addAdminViewModel.Password
            };

            var response = await HttpClient.PostAsJsonAsync(
                "api/superadmin/admins",
                dto
            );

            //  THIS IS MANDATORY
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API Error: {response.StatusCode} - {error}");
            }
        }

        public async Task<AdminDto> GetAdminByIdAsync(Guid adminId)
        {
            Authentication();
            return await HttpClient.GetFromJsonAsync<AdminDto>(
                $"api/superadmin/admins/{adminId}"
            )!;
        }

        public async Task UpdateAdminAsync(Guid adminId, AddAdminViewModel addAdminViewModel)
        {
            Authentication();
            var response = await HttpClient.PutAsJsonAsync(
                $"api/superadmin/admins/{adminId}", addAdminViewModel);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeactivateAdminAsync(Guid adminId)
        {
            Authentication();
            var response = await HttpClient.DeleteAsync(
                $"api/superadmin/admins/{adminId}");

            response.EnsureSuccessStatusCode();
        }
        public async Task<List<AdminDto>> GetAdminsAsync()
        {
            Authentication();
            return await HttpClient.GetFromJsonAsync<List<AdminDto>>("api/superadmin/admins");
        }
        public async Task<List<MovieResponse>> GetMoviesAsync()
        {
            Authentication();
            return await HttpClient.GetFromJsonAsync<List<MovieResponse>>("api/superadmin/movies");
        }

        public async Task AddMovieAsync(AddMovieViewModel addMovieViewModel)
        {
            Authentication();

            var response = await HttpClient.PostAsJsonAsync(
                "api/superadmin/movies",
                new AddMovieRequest
                {
                    Title = addMovieViewModel.Title,
                    Description = addMovieViewModel.Description,
                    DurationMinutes = addMovieViewModel.DurationMinutes,
                    ReleaseDate = addMovieViewModel.ReleaseDate,
                    PosterUrl = addMovieViewModel.PosterUrl
                });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"AddMovie failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task<List<TheatreResponse>> GetTheatresAsync()
        {
            Authentication();
            return await HttpClient.GetFromJsonAsync<List<TheatreResponse>>("api/superadmin/theatres");
        }

        public async Task<List<ScreenResponse>> GetScreensByTheatreAsync(Guid theatreId)
        {
            Authentication();
            return await HttpClient.GetFromJsonAsync<List<ScreenResponse>>(
                $"api/superadmin/screens/by-theatre/{theatreId}");
        }

        public async Task AddTheatreAsync(AddTheatreViewModel addTheatreViewModel)
        {
            Authentication();

            var response = await HttpClient.PostAsJsonAsync(
                "api/superadmin/theatres",
                new CreateTheatreRequest
                {
                    Name = addTheatreViewModel.Name,
                    Location = addTheatreViewModel.Location,
                    TimeSlots = addTheatreViewModel.TimeSlots.Select(ts => new TimeSlotRequest
                    {
                        StartTime = ts.StartTime,
                        EndTime = ts.EndTime
                    }).ToList()
                });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"AddTheatre failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task AddScreenAsync(AddScreenViewModel addScreenViewModel)
        {
            Authentication();

            var response = await HttpClient.PostAsJsonAsync(
                "api/superadmin/screens",
                new CreateScreenRequest
                {
                    TheatreId = addScreenViewModel.TheatreId,
                    ScreenName = addScreenViewModel.ScreenName,
                    SeatLayoutType = addScreenViewModel.SeatLayoutType,
                    SeatRows = addScreenViewModel.SeatRows.Select(r => new CreateSeatRowRequest
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
                throw new Exception(
                    $"AddScreen failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task<List<ScreenResponse>> GetScreensAsync()
        {
            Authentication();
            return await HttpClient.GetFromJsonAsync<List<ScreenResponse>>("api/superadmin/screens");
        }

        public async Task<List<ShowTimeResponse>> GetShowTimesAsync()
        {
            Authentication();
            return await HttpClient.GetFromJsonAsync<List<ShowTimeResponse>>("api/superadmin/showtimes");
        }
        public async Task<List<MovieShowTimeViewModel>> GetMovieWiseShowTimesAsync()
        {
            Authentication();

            var showTimes =
                await HttpClient.GetFromJsonAsync<List<ShowTimeResponse>>(
                    "api/superadmin/showtimes");

            return showTimes!
                .GroupBy(x => x.MovieTitle)
                .Select(g => new MovieShowTimeViewModel
                {
                    MovieTitle = g.Key,
                    ShowTimes = g.ToList()
                })
                .ToList();
        }

        public async Task<AddShowTimeViewModel> GetAddShowTimeFormAsync()
        {
            Authentication();
            return new AddShowTimeViewModel
            {
                Movies = await HttpClient.GetFromJsonAsync<List<MovieResponse>>("api/superadmin/movies"),
                Theatres = await HttpClient.GetFromJsonAsync<List<TheatreResponse>>("api/superadmin/theatres"),
                Languages = await HttpClient.GetFromJsonAsync<List<LanguageResponse>>("api/superadmin/languages")
            };
        }

        public async Task<AddShowTimeBulkViewModel> GetAddShowTimeBulkFormAsync()
        {
            Authentication();

            return new AddShowTimeBulkViewModel
            {
                Movies = await HttpClient.GetFromJsonAsync<List<MovieResponse>>("api/superadmin/movies"),
                Theatres = await HttpClient.GetFromJsonAsync<List<TheatreResponse>>("api/superadmin/theatres"),
                Languages = await HttpClient.GetFromJsonAsync<List<LanguageResponse>>("api/superadmin/languages"),
                ShowTimes = new List<ShowTimeItemVm>()
            };
        }

        public async Task AddShowTimeAsync(AddShowTimeViewModel addShowTimeViewModel)
        {
            Authentication();

            var response = await HttpClient.PostAsJsonAsync(
                "api/superadmin/showtimes",
                new CreateShowTimeRequest
                {
                    MovieId = addShowTimeViewModel.MovieId,
                    TheatreId = addShowTimeViewModel.TheatreId,
                    ScreenId = addShowTimeViewModel.ScreenId,
                    LanguageId = addShowTimeViewModel.LanguageId,
                    ShowDate = addShowTimeViewModel.ShowDate,
                    BasePrice = addShowTimeViewModel.BasePrice
                });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"AddShowTime failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task AddShowTimesBulkAsync(AddShowTimeBulkViewModel addShowTimeBulkViewModel)
        {
            Authentication();

            foreach (var item in addShowTimeBulkViewModel.ShowTimes)
            {
                var response = await HttpClient.PostAsJsonAsync(
                    "api/superadmin/showtimes",
                    new CreateShowTimeRequest
                    {
                        TheatreId = item.TheatreId,
                        ScreenId = item.ScreenId,
                        MovieId = addShowTimeBulkViewModel.MovieId,
                        LanguageId = addShowTimeBulkViewModel.LanguageId,
                        ShowDate = item.ShowDate,
                        BasePrice = item.BasePrice
                    });

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Bulk AddShowTime failed: {error}");
                }
            }
        }

        public async Task<List<AdminRequestResponse>> GetRequestsAsync()
        {
            Authentication();
            return await HttpClient.GetFromJsonAsync<List<AdminRequestResponse>>("api/superadmin/requests");
        }

        public async Task ApproveRequestAsync(Guid id)
        {
            Authentication();
            await HttpClient.PutAsync($"api/superadmin/requests/{id}/approve", null);
        }

        public async Task RejectRequestAsync(Guid id)
        {
            Authentication();
            await HttpClient.PutAsync($"api/superadmin/requests/{id}/reject", null);
        }

        public async Task<List<LanguageResponse>> GetLanguagesAsync()
        {
            Authentication();
            return await HttpClient.GetFromJsonAsync<List<LanguageResponse>>("api/superadmin/languages");
        }


        public async Task<MovieResponse> GetMovieByIdAsync(Guid movieId)
        {
            Authentication();
            return await HttpClient.GetFromJsonAsync<MovieResponse>($"api/superadmin/movies/{movieId}");
        }

        public async Task<TheatreResponse> GetTheatreByIdAsync(Guid theatreId)
        {
            Authentication();
            return await HttpClient.GetFromJsonAsync<TheatreResponse>($"api/superadmin/theatres/{theatreId}");
        }

        public async Task<CreateScreenRequest> GetScreenByIdAsync(Guid screenId)
        {
            Authentication();
            return await HttpClient.GetFromJsonAsync<CreateScreenRequest>($"api/superadmin/screens/{screenId}");
        }

        public async Task<ShowTimeResponse> GetShowTimeByIdAsync(Guid showTimeId)
        {
            Authentication();
            return await HttpClient.GetFromJsonAsync<ShowTimeResponse>($"api/superadmin/showtimes/{showTimeId}");
        }

        public async Task<LanguageResponse> GetLanguageByIdAsync(Guid languageId)
        {
            Authentication();
            return await HttpClient.GetFromJsonAsync<LanguageResponse>($"api/superadmin/languages/{languageId}");
        }

        // ========== NEW: UPDATE METHODS ==========

        public async Task UpdateMovieAsync(Guid movieId, AddMovieViewModel addMovieViewModel)
        {
            Authentication();

            var response = await HttpClient.PutAsJsonAsync(
                $"api/superadmin/movies/{movieId}",
                new AddMovieRequest
                {
                    Title = addMovieViewModel.Title,
                    Description = addMovieViewModel.Description,
                    DurationMinutes = addMovieViewModel.DurationMinutes,
                    ReleaseDate = addMovieViewModel.ReleaseDate,
                    PosterUrl = addMovieViewModel.PosterUrl
                });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"UpdateMovie failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task UpdateTheatreAsync(Guid theatreId, AddTheatreViewModel addTheatreViewModel)
        {
            Authentication();

            var response = await HttpClient.PutAsJsonAsync(
                $"api/superadmin/theatres/{theatreId}",
                new CreateTheatreRequest
                {
                    Name = addTheatreViewModel.Name,
                    Location = addTheatreViewModel.Location,
                    TimeSlots = addTheatreViewModel.TimeSlots.Select(ts => new TimeSlotRequest
                    {
                        StartTime = ts.StartTime,
                        EndTime = ts.EndTime
                    }).ToList()
                });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"UpdateTheatre failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task UpdateScreenAsync(Guid screenId, AddScreenViewModel addScreenViewModel)
        {
            Authentication();

            var response = await HttpClient.PutAsJsonAsync(
                $"api/superadmin/screens/{screenId}",
                new CreateScreenRequest
                {
                    TheatreId = addScreenViewModel.TheatreId,
                    ScreenName = addScreenViewModel.ScreenName,
                    SeatLayoutType = addScreenViewModel.SeatLayoutType,
                    SeatRows = addScreenViewModel.SeatRows.Select(r => new CreateSeatRowRequest
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
                throw new Exception(
                    $"UpdateScreen failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task UpdateShowTimeAsync(Guid showTimeId, AddShowTimeViewModel addShowTimeViewModel)
        {
            Authentication();

            var response = await HttpClient.PutAsJsonAsync(
                $"api/superadmin/showtimes/{showTimeId}",
                new CreateShowTimeRequest
                {
                    MovieId = addShowTimeViewModel.MovieId,
                    TheatreId = addShowTimeViewModel.TheatreId,
                    ScreenId = addShowTimeViewModel.ScreenId,
                    LanguageId = addShowTimeViewModel.LanguageId,
                    ShowDate = addShowTimeViewModel.ShowDate,
                    BasePrice = addShowTimeViewModel.BasePrice
                });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"UpdateShowTime failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task UpdateLanguageAsync(Guid languageId, string name)
        {
            Authentication();

            var response = await HttpClient.PutAsJsonAsync(
                $"api/superadmin/languages/{languageId}",
                new { Name = name });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"UpdateLanguage failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        // ========== NEW: DELETE METHODS ==========

        public async Task DeleteMovieAsync(Guid movieId)
        {
            Authentication();

            var response = await HttpClient.DeleteAsync($"api/superadmin/movies/{movieId}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"DeleteMovie failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task DeleteTheatreAsync(Guid theatreId)
        {
            Authentication();

            var response = await HttpClient.DeleteAsync($"api/superadmin/theatres/{theatreId}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"DeleteTheatre failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task DeleteScreenAsync(Guid screenId)
        {
            Authentication();

            var response = await HttpClient.DeleteAsync($"api/superadmin/screens/{screenId}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"DeleteScreen failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task DeleteShowTimeAsync(Guid showTimeId)
        {
            Authentication();

            var response = await HttpClient.DeleteAsync($"api/superadmin/showtimes/{showTimeId}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"DeleteShowTime failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task DeleteLanguageAsync(Guid languageId)
        {
            Authentication();

            var response = await HttpClient.DeleteAsync($"api/superadmin/languages/{languageId}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"DeleteLanguage failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }

        public async Task AddLanguageAsync(string name)
        {
            Authentication();

            var response = await HttpClient.PostAsJsonAsync(
                "api/superadmin/languages",
                new { Name = name });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(
                    $"AddLanguage failed: {(int)response.StatusCode} {response.StatusCode} - {error}");
            }
        }
    }
}