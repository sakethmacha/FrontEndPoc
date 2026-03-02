using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Web.Interfaces;
using MovieBooking.Web.ViewModels;

namespace MovieBooking.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class ShowTimeController : Controller
    {
        private readonly IShowTimeMvcService ShowTimeService;
        private readonly IMovieMvcService MovieService;
        private readonly ITheatreMvcService TheatreService;
        private readonly ILanguageMvcService LanguageService;

        public ShowTimeController(
            IShowTimeMvcService showTimeService,
            IMovieMvcService movieService,
            ITheatreMvcService theatreService,
            ILanguageMvcService languageService)
        {
            ShowTimeService = showTimeService;
            MovieService = movieService;
            TheatreService = theatreService;
            LanguageService = languageService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
            => View(await ShowTimeService.GetMovieWiseShowTimesAsync());

        [HttpGet]
        public async Task<IActionResult> AddShowTime()
            => View(await ShowTimeService.GetAddShowTimeBulkFormAsync());

        [HttpPost]
        public async Task<IActionResult> AddShowTime(AddShowTimeBulkViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            try
            {
                await ShowTimeService.AddShowTimesBulkAsync(vm);
                TempData["Success"] = "ShowTimes added successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditShowTime(Guid id)
        {
            var showTime = await ShowTimeService.GetShowTimeByIdAsync(id);
            var vm = new AddShowTimeBulkViewModel
            {
                Movies = await MovieService.GetMoviesAsync(),
                Theatres = await TheatreService.GetTheatresAsync(),
                Languages = await LanguageService.GetLanguagesAsync()
            };
            ViewBag.ShowTimeId = id;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditShowTime(Guid id, AddShowTimeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Movies = await MovieService.GetMoviesAsync();
                vm.Theatres = await TheatreService.GetTheatresAsync();
                vm.Languages = await LanguageService.GetLanguagesAsync();
                ViewBag.ShowTimeId = id;
                return View(vm);
            }
            try
            {
                await ShowTimeService.UpdateShowTimeAsync(id, vm);
                TempData["Success"] = "ShowTime updated successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(vm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteShowTime(Guid id)
        {
            try
            {
                await ShowTimeService.DeleteShowTimeAsync(id);
                TempData["Success"] = "ShowTime deleted successfully";
            }
            catch (Exception ex) { TempData["Error"] = ex.Message; }
            return RedirectToAction("Index");
        }
    }
}