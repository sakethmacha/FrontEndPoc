using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Web.Interfaces;
using MovieBooking.Web.ViewModels.Admin;

namespace MovieBooking.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RequestController : Controller
    {
        private readonly IAdminMvcService AdminMvcService;

        public RequestController(IAdminMvcService adminService)
        {
            AdminMvcService = adminService;
        }


        public IActionResult Dashboard() => View();

        // ========== THEATRE REQUESTS ==========

        [HttpGet]
        public async Task<IActionResult> Theatres()
        {
            var requests = await AdminMvcService.GetTheatreRequestsAsync();
            return View(requests);
        }

        [HttpGet]
        public IActionResult RequestTheatre() => View();

        [HttpPost]
        public async Task<IActionResult> RequestTheatre(RequestTheatreViewModel requestTheatreViewModel)
        {
            if (!ModelState.IsValid)
                return View(requestTheatreViewModel);

            try
            {
                await AdminMvcService.RequestTheatreAsync(requestTheatreViewModel);
                TempData["Success"] = "Theatre request submitted successfully! Waiting for SuperAdmin approval.";
                return RedirectToAction("Theatres");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(requestTheatreViewModel);
            }
        }

        // ========== SCREEN REQUESTS ==========

        [HttpGet]
        public async Task<IActionResult> Screens()
        {
            var screens = await AdminMvcService.GetScreenRequestsAsync();
            return View(screens);
        }

        [HttpGet]
        public async Task<IActionResult> RequestScreen()
        {
            var vm = new RequestScreenViewModel
            {
                AvailableTheatres = await AdminMvcService.GetTheatresForScreenAsync()
            };

            if (!vm.AvailableTheatres.Any())
            {
                TempData["Error"] = "You don't have any approved theatres. Please request a theatre first.";
                return RedirectToAction("Theatres");
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> RequestScreen(RequestScreenViewModel requestScreenViewModel)
        {
            if (!ModelState.IsValid)
            {
                requestScreenViewModel.AvailableTheatres = await AdminMvcService.GetTheatresForScreenAsync();
                return View(requestScreenViewModel);
            }

            try
            {
                await AdminMvcService.RequestScreenAsync(requestScreenViewModel);
                TempData["Success"] = "Screen request submitted successfully! Waiting for SuperAdmin approval.";
                return RedirectToAction("Screens");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                requestScreenViewModel.AvailableTheatres = await AdminMvcService.GetTheatresForScreenAsync();
                return View(requestScreenViewModel);
            }
        }

        // ========== APPROVED ITEMS ==========

        [HttpGet]
        public async Task<IActionResult> ApprovedTheatres()
        {
            var theatres = await AdminMvcService.GetApprovedTheatresAsync();
            return View(theatres);
        }

        [HttpGet]
        public async Task<IActionResult> ApprovedScreens()
        {
            var screens = await AdminMvcService.GetApprovedScreensAsync();
            return View(screens);
        }
    }
}