using MovieBooking.Web.ApiContracts.AdminRequests;

namespace MovieBooking.Web.Interfaces
{
    public interface IRequestApprovalMvcService
    {
        Task<List<AdminRequestResponse>> GetRequestsAsync();
        Task ApproveRequestAsync(Guid id);
        Task RejectRequestAsync(Guid id);
    }
}