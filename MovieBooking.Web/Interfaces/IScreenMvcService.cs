using MovieBooking.Web.ApiContracts.Screens;
using MovieBooking.Web.ViewModels;

namespace MovieBooking.Web.Interfaces
{
    public interface IScreenMvcService
    {
        Task<List<ScreenResponse>> GetScreensAsync();
        Task<CreateScreenRequest> GetScreenByIdAsync(Guid screenId);
        Task<List<ScreenResponse>> GetScreensByTheatreAsync(Guid theatreId);
        Task AddScreenAsync(AddScreenViewModel vm);
        Task UpdateScreenAsync(Guid screenId, AddScreenViewModel vm);
        Task DeleteScreenAsync(Guid screenId);
    }
}