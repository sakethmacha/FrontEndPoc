using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Web.Interfaces;
using MovieBooking.Web.ViewModels.Admin;

namespace MovieBooking.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminMvcService AdminMvcService;

        public AdminController(IAdminMvcService adminService)
        {
            AdminMvcService = adminService;
        }


        public IActionResult Dashboard() => View();

        // ========== THEATRE REQUESTS ==========

        [HttpGet]
        public async Task<IActionResult> MyTheatres()
        {
            var requests = await AdminMvcService.GetMyTheatreRequestsAsync();
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
                return RedirectToAction("MyTheatres");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(requestTheatreViewModel);
            }
        }

        // ========== SCREEN REQUESTS ==========

        [HttpGet]
        public async Task<IActionResult> MyScreens()
        {
            var screens = await AdminMvcService.GetMyScreenRequestsAsync();
            return View(screens);
        }

        [HttpGet]
        public async Task<IActionResult> RequestScreen()
        {
            var vm = new RequestScreenViewModel
            {
                AvailableTheatres = await AdminMvcService.GetMyTheatresForScreenAsync()
            };

            if (!vm.AvailableTheatres.Any())
            {
                TempData["Error"] = "You don't have any approved theatres. Please request a theatre first.";
                return RedirectToAction("MyTheatres");
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> RequestScreen(RequestScreenViewModel requestScreenViewModel)
        {
            if (!ModelState.IsValid)
            {
                requestScreenViewModel.AvailableTheatres = await AdminMvcService.GetMyTheatresForScreenAsync();
                return View(requestScreenViewModel);
            }

            try
            {
                await AdminMvcService.RequestScreenAsync(requestScreenViewModel);
                TempData["Success"] = "Screen request submitted successfully! Waiting for SuperAdmin approval.";
                return RedirectToAction("MyScreens");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                requestScreenViewModel.AvailableTheatres = await AdminMvcService.GetMyTheatresForScreenAsync();
                return View(requestScreenViewModel);
            }
        }

        // ========== APPROVED ITEMS ==========

        [HttpGet]
        public async Task<IActionResult> ApprovedTheatres()
        {
            var theatres = await AdminMvcService.GetMyApprovedTheatresAsync();
            return View(theatres);
        }

        [HttpGet]
        public async Task<IActionResult> ApprovedScreens()
        {
            var screens = await AdminMvcService.GetMyApprovedScreensAsync();
            return View(screens);
        }
    }
}