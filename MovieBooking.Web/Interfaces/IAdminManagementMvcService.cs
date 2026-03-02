using MovieBooking.Web.ApiContracts.Admin;
using MovieBooking.Web.ViewModels;

namespace MovieBooking.Web.Interfaces
{
    public interface IAdminManagementMvcService
    {
        Task<List<AdminDto>> GetAdminsAsync();
        Task<AdminDto> GetAdminByIdAsync(Guid adminId);
        Task CreateAdminAsync(AddAdminViewModel vm);
        Task UpdateAdminAsync(Guid adminId, AddAdminViewModel vm);
        Task DeactivateAdminAsync(Guid adminId);
    }
}