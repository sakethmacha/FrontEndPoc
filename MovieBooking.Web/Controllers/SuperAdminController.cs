using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Web.Interfaces;
using MovieBooking.Web.ViewModels;

namespace MovieBooking.Web.Controllers
{
   // [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminController : Controller
    {
        private readonly ISuperAdminMvcService _service;

        public SuperAdminController(ISuperAdminMvcService service)
        {
            _service = service;
        }

        public IActionResult Dashboard() => View();

        // ========== MOVIES ==========

        [HttpGet("movies")]
        public async Task<IActionResult> Movies()
            => View(await _service.GetMoviesAsync());

        public IActionResult AddMovie() => View();

        [HttpPost]
        public async Task<IActionResult> AddMovie(AddMovieViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            await _service.AddMovieAsync(vm);
            return RedirectToAction("Movies");
        }

        [HttpGet]
        public async Task<IActionResult> EditMovie(Guid id)
        {
            var movie = await _service.GetMovieByIdAsync(id);
            var vm = new AddMovieViewModel
            {
                Title = movie.Title,
                Description = movie.Description ?? string.Empty,
                DurationMinutes = movie.DurationMinutes,
                ReleaseDate = movie.ReleaseDate,
                PosterUrl = string.Empty
            };
            ViewBag.MovieId = id;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditMovie(Guid id, AddMovieViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MovieId = id;
                return View(vm);
            }

            await _service.UpdateMovieAsync(id, vm);
            return RedirectToAction("Movies");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMovie(Guid id)
        {
            try
            {
                await _service.DeleteMovieAsync(id);
                TempData["Success"] = "Movie deleted successfully";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Movies");
        }

        // ========== THEATRES ==========

        public async Task<IActionResult> Theatres()
        {
            return View(await _service.GetTheatresAsync());
        }

        public IActionResult AddTheatre() => View();

        [HttpPost]
        public async Task<IActionResult> AddTheatre(AddTheatreViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            await _service.AddTheatreAsync(vm);
            return RedirectToAction("Theatres");
        }

        [HttpGet]
        public async Task<IActionResult> EditTheatre(Guid id)
        {
            var theatre = await _service.GetTheatreByIdAsync(id);
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
        public async Task<IActionResult> EditTheatre(Guid id, AddTheatreViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TheatreId = id;
                return View(vm);
            }

            await _service.UpdateTheatreAsync(id, vm);
            return RedirectToAction("Theatres");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTheatre(Guid id)
        {
            try
            {
                await _service.DeleteTheatreAsync(id);
                TempData["Success"] = "Theatre deleted successfully";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Theatres");
        }

        // ========== SCREENS ==========

        public async Task<IActionResult> Screens()
        {
            return View(await _service.GetScreensAsync());
        }

        public async Task<IActionResult> AddScreen()
            => View(new AddScreenViewModel
            {
                Theatres = await _service.GetTheatresAsync()
            });

        [HttpPost]
        public async Task<IActionResult> AddScreen(AddScreenViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Theatres = await _service.GetTheatresAsync();
                return View(vm);
            }

            await _service.AddScreenAsync(vm);
            return RedirectToAction("Screens");
        }

        [HttpGet]
        public async Task<IActionResult> EditScreen(Guid id)
        {
            var screen = await _service.GetScreenByIdAsync(id);
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
                Theatres = await _service.GetTheatresAsync()
            };
            ViewBag.ScreenId = id;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditScreen(Guid id, AddScreenViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Theatres = await _service.GetTheatresAsync();
                ViewBag.ScreenId = id;
                return View(vm);
            }

            await _service.UpdateScreenAsync(id, vm);
            return RedirectToAction("Screens");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteScreen(Guid id)
        {
            try
            {
                await _service.DeleteScreenAsync(id);
                TempData["Success"] = "Screen deleted successfully";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Screens");
        }

        [HttpGet]
        public async Task<IActionResult> GetScreensByTheatre(Guid theatreId)
        {
            if (theatreId == Guid.Empty)
                return Json(new List<object>());
            var screens = await _service.GetScreensByTheatreAsync(theatreId);
            return Json(screens);
        }

        // ========== SHOWTIMES ==========

        public async Task<IActionResult> ShowTimes()
            => View(await _service.GetShowTimesAsync());

        [HttpGet]
        public async Task<IActionResult> AddShowTime()
        {
            return View(await _service.GetAddShowTimeBulkFormAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AddShowTime(AddShowTimeBulkViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            await _service.AddShowTimesBulkAsync(vm);
            return RedirectToAction("ShowTimes");
        }

        [HttpGet]
        public async Task<IActionResult> EditShowTime(Guid id)
        {
            var showTime = await _service.GetShowTimeByIdAsync(id);
            var vm = new AddShowTimeViewModel
            {
                MovieId = Guid.Empty,
                TheatreId = Guid.Empty,
                ScreenId = Guid.Empty,
                LanguageId = Guid.Empty,
                ShowDate = DateOnly.FromDateTime(showTime.StartTime),
                BasePrice = showTime.BasePrice,
                Movies = await _service.GetMoviesAsync(),
                Theatres = await _service.GetTheatresAsync(),
                Languages = await _service.GetLanguagesAsync()
            };
            ViewBag.ShowTimeId = id;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditShowTime(Guid id, AddShowTimeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Movies = await _service.GetMoviesAsync();
                vm.Theatres = await _service.GetTheatresAsync();
                vm.Languages = await _service.GetLanguagesAsync();
                ViewBag.ShowTimeId = id;
                return View(vm);
            }

            await _service.UpdateShowTimeAsync(id, vm);
            return RedirectToAction("ShowTimes");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteShowTime(Guid id)
        {
            try
            {
                await _service.DeleteShowTimeAsync(id);
                TempData["Success"] = "ShowTime deleted successfully";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("ShowTimes");
        }

        // ========== LANGUAGES ==========

        public async Task<IActionResult> Languages()
        {
            return View(await _service.GetLanguagesAsync());
        }

        public IActionResult AddLanguage() => View();

        [HttpPost]
        public async Task<IActionResult> AddLanguage(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError("", "Language name is required");
                return View();
            }

            await _service.AddLanguageAsync(name);
            return RedirectToAction("Languages");
        }

        [HttpGet]
        public async Task<IActionResult> EditLanguage(Guid id)
        {
            var language = await _service.GetLanguageByIdAsync(id);
            ViewBag.LanguageId = id;
            ViewBag.LanguageName = language.Name;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditLanguage(Guid id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError("", "Language name is required");
                ViewBag.LanguageId = id;
                return View();
            }

            await _service.UpdateLanguageAsync(id, name);
            return RedirectToAction("Languages");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLanguage(Guid id)
        {
            try
            {
                await _service.DeleteLanguageAsync(id);
                TempData["Success"] = "Language deleted successfully";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Languages");
        }

        // ========== REQUESTS ==========

        public async Task<IActionResult> Requests()
            => View(await _service.GetRequestsAsync());

        public async Task<IActionResult> Approve(Guid id)
        {
            await _service.ApproveRequestAsync(id);
            return RedirectToAction("Requests");
        }

        public async Task<IActionResult> Reject(Guid id)
        {
            await _service.RejectRequestAsync(id);
            return RedirectToAction("Requests");
        }
    }
}