using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Web.Interfaces;
using MovieBooking.Web.ViewModels;

namespace MovieBooking.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class ScreenController : Controller
    {
        private readonly IScreenMvcService ScreenService;
        private readonly ITheatreMvcService TheatreService;

        public ScreenController(IScreenMvcService screenService, ITheatreMvcService theatreService)
        {
            ScreenService = screenService;
            TheatreService = theatreService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
            => View(await ScreenService.GetScreensAsync());

        public async Task<IActionResult> AddScreen()
            => View(new AddScreenViewModel
            {
                Theatres = await TheatreService.GetTheatresAsync()
            });

        [HttpPost]
        public async Task<IActionResult> AddScreen(AddScreenViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Theatres = await TheatreService.GetTheatresAsync();
                return View(vm);
            }
            try
            {
                await ScreenService.AddScreenAsync(vm);
                TempData["Success"] = "Screen added successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                vm.Theatres = await TheatreService.GetTheatresAsync();
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditScreen(Guid id)
        {
            var screen = await ScreenService.GetScreenByIdAsync(id);
            var vm = new AddScreenViewModel
            {
                TheatreId = screen.TheatreId,
                ScreenName = screen.ScreenName,
                SeatLayoutType = screen.SeatLayoutType,
                SeatRows = screen.SeatRows.Select(sr => new SeatRowViewModel
                {
                    SeatRow = sr.SeatRow,
                    SeatCount = sr.SeatCount,
                    SeatType = sr.SeatType,
                    PriceMultiplier = sr.PriceMultiplier
                }).ToList(),
                Theatres = await TheatreService.GetTheatresAsync()
            };
            ViewBag.ScreenId = id;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditScreen(Guid id, AddScreenViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Theatres = await TheatreService.GetTheatresAsync();
                ViewBag.ScreenId = id;
                return View(vm);
            }
            try
            {
                await ScreenService.UpdateScreenAsync(id, vm);
                TempData["Success"] = "Screen updated successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                vm.Theatres = await TheatreService.GetTheatresAsync();
                return View(vm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteScreen(Guid id)
        {
            try
            {
                await ScreenService.DeleteScreenAsync(id);
                TempData["Success"] = "Screen deleted successfully";
            }
            catch (Exception ex) { TempData["Error"] = ex.Message; }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetScreensByTheatre(Guid theatreId)
        {
            if (theatreId == Guid.Empty) return Json(new List<object>());
            var screens = await ScreenService.GetScreensByTheatreAsync(theatreId);
            return View(screens);
        }

        [HttpGet]
        public async Task<IActionResult> ScreensByTheatre(Guid theatreId)
        {
            if (theatreId == Guid.Empty) return Json(new List<object>());
            var screens = await ScreenService.GetScreensByTheatreAsync(theatreId);
            return Ok(screens);
        }
    }
}