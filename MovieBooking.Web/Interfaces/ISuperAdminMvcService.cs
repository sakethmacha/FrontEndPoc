using MovieBooking.Web.ApiContracts.AdminRequests;
using MovieBooking.Web.ApiContracts.Movies;
using MovieBooking.Web.ApiContracts.Screens;
using MovieBooking.Web.ApiContracts.ShowTimes;
using MovieBooking.Web.ApiContracts.Theatres;
using MovieBooking.Web.ViewModels;

namespace MovieBooking.Web.Interfaces
{
    public interface ISuperAdminMvcService
    {
        Task<List<MovieResponse>> GetMoviesAsync();
        Task AddMovieAsync(AddMovieViewModel vm);

        Task<List<TheatreResponse>> GetTheatresAsync();
        Task AddTheatreAsync(AddTheatreViewModel vm);
        Task<List<ScreenResponse>> GetScreensByTheatreAsync(Guid theatreId);
        Task AddScreenAsync(AddScreenViewModel vm);

        Task<List<ShowTimeResponse>> GetShowTimesAsync();
        Task<AddShowTimeViewModel> GetAddShowTimeFormAsync();
        Task AddShowTimeAsync(AddShowTimeViewModel vm);

        Task<List<AdminRequestResponse>> GetRequestsAsync();
        Task ApproveRequestAsync(Guid id);
        Task RejectRequestAsync(Guid id);
        Task AddShowTimesBulkAsync(AddShowTimeBulkViewModel vm);

        Task<AddShowTimeBulkViewModel> GetAddShowTimeBulkFormAsync();
    }

}
