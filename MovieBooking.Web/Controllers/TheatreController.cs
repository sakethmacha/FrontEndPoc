using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Web.Interfaces;
using MovieBooking.Web.ViewModels;

namespace MovieBooking.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class TheatreController : Controller
    {
        private readonly ITheatreMvcService TheatreService;

        public TheatreController(ITheatreMvcService theatreService)
        {
            TheatreService = theatreService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
            => View(await TheatreService.GetTheatresAsync());

        public IActionResult AddTheatre() => View();

        [HttpPost]
        public async Task<IActionResult> AddTheatre(AddTheatreViewModel addTheatreViewModel)
        {
            if (!ModelState.IsValid) return View(addTheatreViewModel);
            try
            {
                await TheatreService.AddTheatreAsync(addTheatreViewModel);
                TempData["Success"] = "Theatre added successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(addTheatreViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditTheatre(Guid id)
        {
            var theatre = await TheatreService.GetTheatreByIdAsync(id);
            var vm = new AddTheatreViewModel
            {
                Name = theatre.Name,
                Location = theatre.Location,
                TimeSlots = new List<TimeSlotViewModel>()
            };
            ViewBag.TheatreId = id;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditTheatre(Guid id, AddTheatreViewModel addTheatreViewModel)
        {
            if (!ModelState.IsValid) { ViewBag.TheatreId = id; return View(addTheatreViewModel); }
            try
            {
                await TheatreService.UpdateTheatreAsync(id, addTheatreViewModel);
                TempData["Success"] = "Theatre updated successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(addTheatreViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTheatre(Guid id)
        {
            try
            {
                await TheatreService.DeleteTheatreAsync(id);
                TempData["Success"] = "Theatre deleted successfully";
            }
            catch (Exception ex) { TempData["Error"] = ex.Message; }
            return RedirectToAction("Index");
        }
    }
}