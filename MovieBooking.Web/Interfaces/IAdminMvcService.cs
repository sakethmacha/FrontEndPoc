using MovieBooking.Web.ApiContracts.Admin;
using MovieBooking.Web.ViewModels.Admin;

namespace MovieBooking.Web.Interfaces
{
    public interface IAdminMvcService
    {
        // Theatre Requests
        Task RequestTheatreAsync(RequestTheatreViewModel vm);
        Task<List<TheatreRequestResponse>> GetMyTheatreRequestsAsync();
        Task<List<TheatreRequestResponse>> GetMyApprovedTheatresAsync();

        // Screen Requests
        Task RequestScreenAsync(RequestScreenViewModel vm);
        Task<List<ScreenRequestResponse>> GetMyScreenRequestsAsync();
        Task<List<ScreenRequestResponse>> GetMyApprovedScreensAsync();

        // Helper
        Task<List<TheatreDropdownItem>> GetMyTheatresForScreenAsync();
    }
}