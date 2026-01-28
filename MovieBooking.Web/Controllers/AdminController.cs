using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Web.Interfaces;
using MovieBooking.Web.ViewModels.Admin;

namespace MovieBooking.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminMvcService _service;

        public AdminController(IAdminMvcService service)
        {
            _service = service;
        }

        // ========== DASHBOARD ==========

        public IActionResult Dashboard() => View();

        // ========== THEATRE REQUESTS ==========

        [HttpGet]
        public async Task<IActionResult> MyTheatres()
        {
            var requests = await _service.GetMyTheatreRequestsAsync();
            return View(requests);
        }

        [HttpGet]
        public IActionResult RequestTheatre() => View();

        [HttpPost]
        public async Task<IActionResult> RequestTheatre(RequestTheatreViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                await _service.RequestTheatreAsync(vm);
                TempData["Success"] = "Theatre request submitted successfully! Waiting for SuperAdmin approval.";
                return RedirectToAction("MyTheatres");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(vm);
            }
        }

        // ========== SCREEN REQUESTS ==========

        [HttpGet]
        public async Task<IActionResult> MyScreens()
        {
            var screens = await _service.GetMyScreenRequestsAsync();
            return View(screens);
        }

        [HttpGet]
        public async Task<IActionResult> RequestScreen()
        {
            var vm = new RequestScreenViewModel
            {
                AvailableTheatres = await _service.GetMyTheatresForScreenAsync()
            };

            if (!vm.AvailableTheatres.Any())
            {
                TempData["Error"] = "You don't have any approved theatres. Please request a theatre first.";
                return RedirectToAction("MyTheatres");
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> RequestScreen(RequestScreenViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.AvailableTheatres = await _service.GetMyTheatresForScreenAsync();
                return View(vm);
            }

            try
            {
                await _service.RequestScreenAsync(vm);
                TempData["Success"] = "Screen request submitted successfully! Waiting for SuperAdmin approval.";
                return RedirectToAction("MyScreens");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                vm.AvailableTheatres = await _service.GetMyTheatresForScreenAsync();
                return View(vm);
            }
        }

        // ========== APPROVED ITEMS ==========

        [HttpGet]
        public async Task<IActionResult> ApprovedTheatres()
        {
            var theatres = await _service.GetMyApprovedTheatresAsync();
            return View(theatres);
        }

        [HttpGet]
        public async Task<IActionResult> ApprovedScreens()
        {
            var screens = await _service.GetMyApprovedScreensAsync();
            return View(screens);
        }
    }
}