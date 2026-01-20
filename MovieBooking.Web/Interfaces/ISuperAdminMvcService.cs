using MovieBooking.Web.ApiContracts.AdminRequests;
using MovieBooking.Web.ApiContracts.Languages;
using MovieBooking.Web.ApiContracts.Movies;
using MovieBooking.Web.ApiContracts.Screens;
using MovieBooking.Web.ApiContracts.ShowTimes;
using MovieBooking.Web.ApiContracts.Theatres;
using MovieBooking.Web.ViewModels;

namespace MovieBooking.Web.Interfaces
{
    public interface ISuperAdminMvcService
    {
        // Existing methods
        Task<List<MovieResponse>> GetMoviesAsync();
        Task AddMovieAsync(AddMovieViewModel vm);
        Task<List<TheatreResponse>> GetTheatresAsync();
        Task AddTheatreAsync(AddTheatreViewModel vm);
        Task<List<ScreenResponse>> GetScreensByTheatreAsync(Guid theatreId);
        Task AddScreenAsync(AddScreenViewModel vm);
        Task<List<ShowTimeResponse>> GetShowTimesAsync();
        Task<AddShowTimeViewModel> GetAddShowTimeFormAsync();
        Task<AddShowTimeBulkViewModel> GetAddShowTimeBulkFormAsync();
        Task AddShowTimeAsync(AddShowTimeViewModel vm);
        Task AddShowTimesBulkAsync(AddShowTimeBulkViewModel vm);
        Task<List<AdminRequestResponse>> GetRequestsAsync();
        Task ApproveRequestAsync(Guid id);
        Task RejectRequestAsync(Guid id);

        // ========== NEW: GET BY ID METHODS ==========
        Task<MovieResponse> GetMovieByIdAsync(Guid movieId);
        Task<TheatreResponse> GetTheatreByIdAsync(Guid theatreId);
        Task<CreateScreenRequest> GetScreenByIdAsync(Guid screenId);
        Task<ShowTimeResponse> GetShowTimeByIdAsync(Guid showTimeId);
        Task<LanguageResponse> GetLanguageByIdAsync(Guid languageId);

        // ========== NEW: UPDATE METHODS ==========
        Task UpdateMovieAsync(Guid movieId, AddMovieViewModel vm);
        Task UpdateTheatreAsync(Guid theatreId, AddTheatreViewModel vm);
        Task UpdateScreenAsync(Guid screenId, AddScreenViewModel vm);
        Task UpdateShowTimeAsync(Guid showTimeId, AddShowTimeViewModel vm);
        Task UpdateLanguageAsync(Guid languageId, string name);

        // ========== NEW: DELETE METHODS ==========
        Task DeleteMovieAsync(Guid movieId);
        Task DeleteTheatreAsync(Guid theatreId);
        Task DeleteScreenAsync(Guid screenId);
        Task DeleteShowTimeAsync(Guid showTimeId);
        Task DeleteLanguageAsync(Guid languageId);

        // ========== NEW: LANGUAGE METHODS ==========
        Task<List<LanguageResponse>> GetLanguagesAsync();
        Task AddLanguageAsync(string name);

        // ========== NEW: SCREEN METHODS ==========
        Task<List<ScreenResponse>> GetScreensAsync();
    }
}