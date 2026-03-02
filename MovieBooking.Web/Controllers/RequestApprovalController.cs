using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Web.Interfaces;

namespace MovieBooking.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class RequestApprovalController : Controller
    {
        private readonly IRequestApprovalMvcService RequestApprovalService;

        public RequestApprovalController(IRequestApprovalMvcService requestApprovalService)
        {
            RequestApprovalService = requestApprovalService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
            => View(await RequestApprovalService.GetRequestsAsync());

        public async Task<IActionResult> Approve(Guid id)
        {
            try
            {
                await RequestApprovalService.ApproveRequestAsync(id);
                TempData["Success"] = "Request approved successfully";
            }
            catch (Exception ex) { TempData["Error"] = ex.Message; }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Reject(Guid id)
        {
            try
            {
                await RequestApprovalService.RejectRequestAsync(id);
                TempData["Success"] = "Request rejected successfully";
            }
            catch (Exception ex) { TempData["Error"] = ex.Message; }
            return RedirectToAction("Index");
        }
    }
}