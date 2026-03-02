using MovieBooking.Web.ApiContracts.Admin;
using MovieBooking.Web.ViewModels.Admin;

namespace MovieBooking.Web.Interfaces
{
    public interface IAdminMvcService
    {
        // Theatre Requests
        Task RequestTheatreAsync(RequestTheatreViewModel vm);
        Task<List<TheatreRequestResponse>> GetTheatreRequestsAsync();
        Task<List<TheatreRequestResponse>> GetApprovedTheatresAsync();

        // Screen Requests
        Task RequestScreenAsync(RequestScreenViewModel vm);
        Task<List<ScreenRequestResponse>> GetScreenRequestsAsync();
        Task<List<ScreenRequestResponse>> GetApprovedScreensAsync();

        // Helper
        Task<List<TheatreDropdownItem>> GetTheatresForScreenAsync();
    }
}