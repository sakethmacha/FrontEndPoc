using MovieBooking.Web.ApiContracts.Theatres;
using MovieBooking.Web.ViewModels;

namespace MovieBooking.Web.Interfaces
{
    public interface ITheatreMvcService
    {
        Task<List<TheatreResponse>> GetTheatresAsync();
        Task<TheatreResponse> GetTheatreByIdAsync(Guid theatreId);
        Task AddTheatreAsync(AddTheatreViewModel vm);
        Task UpdateTheatreAsync(Guid theatreId, AddTheatreViewModel vm);
        Task DeleteTheatreAsync(Guid theatreId);
    }
}