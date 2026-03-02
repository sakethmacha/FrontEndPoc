using MovieBooking.Web.ApiContracts.ShowTimes;
using MovieBooking.Web.ViewModels;

namespace MovieBooking.Web.Interfaces
{
    public interface IShowTimeMvcService
    {
        Task<List<ShowTimeResponse>> GetShowTimesAsync();
        Task<ShowTimeResponse> GetShowTimeByIdAsync(Guid showTimeId);
        Task<AddShowTimeViewModel> GetAddShowTimeFormAsync();
        Task<AddShowTimeBulkViewModel> GetAddShowTimeBulkFormAsync();
        Task AddShowTimeAsync(AddShowTimeViewModel vm);
        Task AddShowTimesBulkAsync(AddShowTimeBulkViewModel vm);
        Task UpdateShowTimeAsync(Guid showTimeId, AddShowTimeViewModel vm);
        Task DeleteShowTimeAsync(Guid showTimeId);
        Task<List<MovieShowTimeViewModel>> GetMovieWiseShowTimesAsync();
    }
}