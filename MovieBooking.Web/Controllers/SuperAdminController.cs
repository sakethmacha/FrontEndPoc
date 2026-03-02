//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using MovieBooking.Web.Interfaces;
//using MovieBooking.Web.ViewModels;

//namespace MovieBooking.Web.Controllers
//{
//   [Authorize(Roles = "SuperAdmin")]
//    public class SuperAdminController : Controller
//    {
//        private readonly ISuperAdminMvcService SuperAdminService;

//        public SuperAdminController(ISuperAdminMvcService superAdminService)
//        {
//            SuperAdminService = superAdminService;
//        }

//        public IActionResult Dashboard() => View();

//        [HttpGet]
//        public IActionResult CreateAdmin()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> CreateAdmin(AddAdminViewModel addAdminViewModel)
//        {
//            if (!ModelState.IsValid)
//                return View(addAdminViewModel);

//            try
//            {
//                await SuperAdminService.CreateAdminAsync(addAdminViewModel);

//                TempData["Success"] = "Admin created successfully";
//                return RedirectToAction("GetAdmins");
//            }
//            catch (Exception ex)
//            {
//                TempData["Error"] = ex.Message;
//                return View(addAdminViewModel);
//            }
//        }

//        // ---------- EDIT ADMIN (GET) ----------
//        [HttpGet]
//        public async Task<IActionResult> EditAdmin(Guid id)
//        {
//            var admin = await SuperAdminService.GetAdminByIdAsync(id);

//            var model = new AddAdminViewModel
//            {
//                UserId = admin.UserId,
//                Name = admin.Name,
//                Email = admin.Email
//            };

//            return View(model);
//        }

//        // ---------- EDIT ADMIN (POST) ----------
//        [HttpPost]
//        public async Task<IActionResult> EditAdmin(AddAdminViewModel addAdminViewModel)
//        {
//            if (!ModelState.IsValid)
//                return View(addAdminViewModel);

//            try
//            {
//                await SuperAdminService.UpdateAdminAsync(addAdminViewModel.UserId, addAdminViewModel);

//                TempData["Success"] = "Admin updated successfully";
//                return RedirectToAction("GetAdmins");
//            }
//            catch (Exception ex)
//            {
//                TempData["Error"] = ex.Message;
//                return View(addAdminViewModel);
//            }
//        }

//        // ---------- DELETE (DEACTIVATE) ADMIN ----------
//        [HttpPost]
//        public async Task<IActionResult> DeleteAdmin(Guid id)
//        {
//            try
//            {
//                await SuperAdminService.DeactivateAdminAsync(id);
//                TempData["Success"] = "Admin deactivated successfully";
//            }
//            catch (Exception ex)
//            {
//                TempData["Error"] = ex.Message;
//            }

//            return RedirectToAction("GetAdmins");
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAdmins()
//        {
//            return View(await SuperAdminService.GetAdminsAsync());
//        }
//        // ========== MOVIES ==========

//        [HttpGet("movies")]
//        public async Task<IActionResult> Movies()
//            => View(await SuperAdminService.GetMoviesAsync());

//        public IActionResult AddMovie() => View();

//        [HttpPost]
//        public async Task<IActionResult> AddMovie(AddMovieViewModel addMovieViewModel)
//        {
//            if (!ModelState.IsValid)
//                return View(addMovieViewModel);

//            await SuperAdminService.AddMovieAsync(addMovieViewModel);
//            return RedirectToAction("Movies");
//        }

//        [HttpGet]
//        public async Task<IActionResult> EditMovie(Guid id)
//        {
//            var movie = await SuperAdminService.GetMovieByIdAsync(id);
//            var addMovieViewModel = new AddMovieViewModel
//            {
//                Title = movie.Title,
//                Description = movie.Description ?? string.Empty,
//                DurationMinutes = movie.DurationMinutes,
//                ReleaseDate = movie.ReleaseDate,
//                PosterUrl = movie.PosterUrl
//            };
//            ViewBag.MovieId = id;
//            return View(addMovieViewModel);
//        }

//        [HttpPost]
//        public async Task<IActionResult> EditMovie(Guid id, AddMovieViewModel addMovieViewModel)
//        {
//            if (!ModelState.IsValid)
//            {
//                ViewBag.MovieId = id;
//                return View(addMovieViewModel);
//            }

//            await SuperAdminService.UpdateMovieAsync(id, addMovieViewModel);
//            return RedirectToAction("Movies");
//        }

//        [HttpPost]
//        public async Task<IActionResult> DeleteMovie(Guid id)
//        {
//            try
//            {
//                await SuperAdminService.DeleteMovieAsync(id);
//                TempData["Success"] = "Movie deleted successfully";
//            }
//            catch (Exception ex)
//            {
//                TempData["Error"] = ex.Message;
//            }
//            return RedirectToAction("Movies");
//        }

//        // ========== THEATRES ==========

//        public async Task<IActionResult> Theatres()
//        {
//            return View(await SuperAdminService.GetTheatresAsync());
//        }

//        public IActionResult AddTheatre() => View();

//        [HttpPost]
//        public async Task<IActionResult> AddTheatre(AddTheatreViewModel addTheatreViewModel)
//        {
//            if (!ModelState.IsValid)
//                return View(addTheatreViewModel);

//            await SuperAdminService.AddTheatreAsync(addTheatreViewModel);
//            return RedirectToAction("Theatres");
//        }

//        [HttpGet]
//        public async Task<IActionResult> EditTheatre(Guid id)
//        {
//            var theatre = await SuperAdminService.GetTheatreByIdAsync(id);
//            var addTheatreViewModel = new AddTheatreViewModel
//            {
//                Name = theatre.Name,
//                Location = theatre.Location,
//                TimeSlots = new List<TimeSlotViewModel>()
//            };
//            ViewBag.TheatreId = id;
//            return View(addTheatreViewModel);
//        }

//        [HttpPost]
//        public async Task<IActionResult> EditTheatre(Guid id, AddTheatreViewModel addTheatreViewModel)
//        {
//            if (!ModelState.IsValid)
//            {
//                ViewBag.TheatreId = id;
//                return View(addTheatreViewModel);
//            }

//            await SuperAdminService.UpdateTheatreAsync(id, addTheatreViewModel);
//            return RedirectToAction("Theatres");
//        }

//        [HttpPost]
//        public async Task<IActionResult> DeleteTheatre(Guid id)
//        {
//            try
//            {
//                await SuperAdminService.DeleteTheatreAsync(id);
//                TempData["Success"] = "Theatre deleted successfully";
//            }
//            catch (Exception ex)
//            {
//                TempData["Error"] = ex.Message;
//            }
//            return RedirectToAction("Theatres");
//        }

//        // ========== SCREENS ==========

//        public async Task<IActionResult> Screens()
//        {
//            return View(await SuperAdminService.GetScreensAsync());
//        }

//        public async Task<IActionResult> AddScreen()
//            => View(new AddScreenViewModel
//            {
//                Theatres = await SuperAdminService.GetTheatresAsync()
//            });

//        [HttpPost]
//        public async Task<IActionResult> AddScreen(AddScreenViewModel addScreenViewModel)
//        {
//            if (!ModelState.IsValid)
//            {
//                addScreenViewModel.Theatres = await SuperAdminService.GetTheatresAsync();
//                return View(addScreenViewModel);
//            }

//            await SuperAdminService.AddScreenAsync(addScreenViewModel);
//            return RedirectToAction("Theatres");
//        }

//        [HttpGet]
//        public async Task<IActionResult> EditScreen(Guid id)
//        {
//            var screen = await SuperAdminService.GetScreenByIdAsync(id);
//            var addScreenViewModel = new AddScreenViewModel
//            {
//                TheatreId = screen.TheatreId,
//                ScreenName = screen.ScreenName,
//                SeatLayoutType = screen.SeatLayoutType,
//                SeatRows = screen.SeatRows.Select(sr => new SeatRowViewModel
//                {
//                    SeatRow = sr.SeatRow,
//                    SeatCount = sr.SeatCount,
//                    SeatType = sr.SeatType,
//                    PriceMultiplier = sr.PriceMultiplier
//                }).ToList(),
//                Theatres = await SuperAdminService.GetTheatresAsync()
//            };
//            ViewBag.ScreenId = id;
//            return View(addScreenViewModel);
//        }

//        [HttpPost]
//        public async Task<IActionResult> EditScreen(Guid id, AddScreenViewModel addScreenViewModel)
//        {
//            if (!ModelState.IsValid)
//            {
//                addScreenViewModel.Theatres = await SuperAdminService.GetTheatresAsync();
//                ViewBag.ScreenId = id;
//                return View(addScreenViewModel);
//            }

//            await SuperAdminService.UpdateScreenAsync(id, addScreenViewModel);
//            return RedirectToAction("Screens");
//        }

//        [HttpPost]
//        public async Task<IActionResult> DeleteScreen(Guid id)
//        {
//            try
//            {
//                await SuperAdminService.DeleteScreenAsync(id);
//                TempData["Success"] = "Screen deleted successfully";
//            }
//            catch (Exception ex)
//            {
//                TempData["Error"] = ex.Message;
//            }
//            return RedirectToAction("GetScreenByTheatre");
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetScreensByTheatre(Guid theatreId)
//        {
//            if (theatreId == Guid.Empty)
//                return Json(new List<object>());
//            var screens = await SuperAdminService.GetScreensByTheatreAsync(theatreId);
//            return View(screens);
//        }
//        [HttpGet]
//        public async Task<IActionResult> ScreensByTheatre(Guid theatreId)
//        {
//            if (theatreId == Guid.Empty)
//                return Json(new List<object>());
//            var screens = await SuperAdminService.GetScreensByTheatreAsync(theatreId);
//            return Ok(screens);
//        }
//        // ========== SHOWTIMES ==========

//        //public async Task<IActionResult> ShowTimes()
//        //    => View(await SuperAdminService.GetShowTimesAsync());
//        public async Task<IActionResult> ShowTimes()
//    => View(await SuperAdminService.GetMovieWiseShowTimesAsync());

//        [HttpGet]
//        public async Task<IActionResult> AddShowTime()
//        {
//            return View(await SuperAdminService.GetAddShowTimeBulkFormAsync());
//        }

//        [HttpPost]
//        public async Task<IActionResult> AddShowTime(AddShowTimeBulkViewModel addShowTimeBulkViewModel)
//        {
//            if (!ModelState.IsValid)
//                return View(addShowTimeBulkViewModel);

//            await SuperAdminService.AddShowTimesBulkAsync(addShowTimeBulkViewModel);
//            return RedirectToAction("ShowTimes");
//        }

//        [HttpGet]
//        public async Task<IActionResult> EditShowTime(Guid id)
//        {
//            var showTime = await SuperAdminService.GetShowTimeByIdAsync(id);

//            var addShowTimeBulkViewModel = new AddShowTimeBulkViewModel
//            {
//                Movies = await SuperAdminService.GetMoviesAsync(),
//                Theatres = await SuperAdminService.GetTheatresAsync(),
//                Languages = await SuperAdminService.GetLanguagesAsync()
//            };
//            ViewBag.ShowTimeId = id;
//            return View(addShowTimeBulkViewModel);
//        }

//        [HttpPost]
//        public async Task<IActionResult> EditShowTime(Guid id, AddShowTimeViewModel addShowTimeViewModel)
//        {
//            if (!ModelState.IsValid)
//            {
//                addShowTimeViewModel.Movies = await SuperAdminService.GetMoviesAsync();
//                addShowTimeViewModel.Theatres = await SuperAdminService.GetTheatresAsync();
//                addShowTimeViewModel.Languages = await SuperAdminService.GetLanguagesAsync();
//                ViewBag.ShowTimeId = id;
//                return View(addShowTimeViewModel);
//            }

//            await SuperAdminService.UpdateShowTimeAsync(id, addShowTimeViewModel);
//            return RedirectToAction("ShowTimes");
//        }

//        [HttpPost]
//        public async Task<IActionResult> DeleteShowTime(Guid id)
//        {
//            try
//            {
//                await SuperAdminService.DeleteShowTimeAsync(id);
//                TempData["Success"] = "ShowTime deleted successfully";
//            }
//            catch (Exception ex)
//            {
//                TempData["Error"] = ex.Message;
//            }
//            return RedirectToAction("ShowTimes");
//        }

//        // ========== LANGUAGES ==========

//        public async Task<IActionResult> Languages()
//        {
//            return View(await SuperAdminService.GetLanguagesAsync());
//        }

//        public IActionResult AddLanguage() => View();

//        [HttpPost]
//        public async Task<IActionResult> AddLanguage(string name)
//        {
//            if (string.IsNullOrWhiteSpace(name))
//            {
//                ModelState.AddModelError("", "Language name is required");
//                return View();
//            }

//            await SuperAdminService.AddLanguageAsync(name);
//            return RedirectToAction("Languages");
//        }

//        [HttpGet]
//        public async Task<IActionResult> EditLanguage(Guid id)
//        {
//            var language = await SuperAdminService.GetLanguageByIdAsync(id);
//            ViewBag.LanguageId = id;
//            ViewBag.LanguageName = language.Name;
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> EditLanguage(Guid id, string name)
//        {
//            if (string.IsNullOrWhiteSpace(name))
//            {
//                ModelState.AddModelError("", "Language name is required");
//                ViewBag.LanguageId = id;
//                return View();
//            }

//            await SuperAdminService.UpdateLanguageAsync(id, name);
//            return RedirectToAction("Languages");
//        }

//        [HttpPost]
//        public async Task<IActionResult> DeleteLanguage(Guid id)
//        {
//            try
//            {
//                await SuperAdminService.DeleteLanguageAsync(id);
//                TempData["Success"] = "Language deleted successfully";
//            }
//            catch (Exception ex)
//            {
//                TempData["Error"] = ex.Message;
//            }
//            return RedirectToAction("Languages");
//        }

//        // ========== REQUESTS ==========

//        public async Task<IActionResult> Requests()
//            => View(await SuperAdminService.GetRequestsAsync());

//        public async Task<IActionResult> Approve(Guid id)
//        {
//            await SuperAdminService.ApproveRequestAsync(id);
//            return RedirectToAction("Requests");
//        }

//        public async Task<IActionResult> Reject(Guid id)
//        {
//            await SuperAdminService.RejectRequestAsync(id);
//            return RedirectToAction("Requests");
//        }
//    }
//}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Web.Interfaces;
using MovieBooking.Web.ViewModels;

namespace MovieBooking.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminController : Controller
    {
        private readonly IAdminManagementMvcService _adminService;
        private readonly IMovieMvcService _movieService;
        private readonly ITheatreMvcService _theatreService;
        private readonly IScreenMvcService _screenService;
        private readonly IShowTimeMvcService _showTimeService;
        private readonly ILanguageMvcService _languageService;
        private readonly IRequestApprovalMvcService _requestApprovalService;

        public SuperAdminController(
            IAdminManagementMvcService adminService,
            IMovieMvcService movieService,
            ITheatreMvcService theatreService,
            IScreenMvcService screenService,
            IShowTimeMvcService showTimeService,
            ILanguageMvcService languageService,
            IRequestApprovalMvcService requestApprovalService)
        {
            _adminService = adminService;
            _movieService = movieService;
            _theatreService = theatreService;
            _screenService = screenService;
            _showTimeService = showTimeService;
            _languageService = languageService;
            _requestApprovalService = requestApprovalService;
        }

        public IActionResult Dashboard() => View();

        // ========== ADMINS ==========

        [HttpGet]
        public async Task<IActionResult> GetAdmins()
            => View(await _adminService.GetAdminsAsync());

        [HttpGet]
        public IActionResult CreateAdmin() => View();

        [HttpPost]
        public async Task<IActionResult> CreateAdmin(AddAdminViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            try
            {
                await _adminService.CreateAdminAsync(vm);
                TempData["Success"] = "Admin created successfully";
                return RedirectToAction("GetAdmins");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditAdmin(Guid id)
        {
            var admin = await _adminService.GetAdminByIdAsync(id);
            var model = new AddAdminViewModel
            {
                UserId = admin.UserId,
                Name = admin.Name,
                Email = admin.Email
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditAdmin(AddAdminViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            try
            {
                await _adminService.UpdateAdminAsync(vm.UserId, vm);
                TempData["Success"] = "Admin updated successfully";
                return RedirectToAction("GetAdmins");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(vm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAdmin(Guid id)
        {
            try
            {
                await _adminService.DeactivateAdminAsync(id);
                TempData["Success"] = "Admin deactivated successfully";
            }
            catch (Exception ex) { TempData["Error"] = ex.Message; }
            return RedirectToAction("GetAdmins");
        }

        // ========== MOVIES ==========

        [HttpGet]
        public async Task<IActionResult> Movies()
            => View(await _movieService.GetMoviesAsync());

        public IActionResult AddMovie() => View();

        [HttpPost]
        public async Task<IActionResult> AddMovie(AddMovieViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            await _movieService.AddMovieAsync(vm);
            return RedirectToAction("Movies");
        }

        [HttpGet]
        public async Task<IActionResult> EditMovie(Guid id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            var vm = new AddMovieViewModel
            {
                Title = movie.Title,
                Description = movie.Description ?? string.Empty,
                DurationMinutes = movie.DurationMinutes,
                ReleaseDate = movie.ReleaseDate,
                PosterUrl = movie.PosterUrl
            };
            ViewBag.MovieId = id;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditMovie(Guid id, AddMovieViewModel vm)
        {
            if (!ModelState.IsValid) { ViewBag.MovieId = id; return View(vm); }
            await _movieService.UpdateMovieAsync(id, vm);
            return RedirectToAction("Movies");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMovie(Guid id)
        {
            try
            {
                await _movieService.DeleteMovieAsync(id);
                TempData["Success"] = "Movie deleted successfully";
            }
            catch (Exception ex) { TempData["Error"] = ex.Message; }
            return RedirectToAction("Movies");
        }

        // ========== THEATRES ==========

        public async Task<IActionResult> Theatres()
            => View(await _theatreService.GetTheatresAsync());

        public IActionResult AddTheatre() => View();

        [HttpPost]
        public async Task<IActionResult> AddTheatre(AddTheatreViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            await _theatreService.AddTheatreAsync(vm);
            return RedirectToAction("Theatres");
        }

        [HttpGet]
        public async Task<IActionResult> EditTheatre(Guid id)
        {
            var theatre = await _theatreService.GetTheatreByIdAsync(id);
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
            if (!ModelState.IsValid) { ViewBag.TheatreId = id; return View(vm); }
            await _theatreService.UpdateTheatreAsync(id, vm);
            return RedirectToAction("Theatres");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTheatre(Guid id)
        {
            try
            {
                await _theatreService.DeleteTheatreAsync(id);
                TempData["Success"] = "Theatre deleted successfully";
            }
            catch (Exception ex) { TempData["Error"] = ex.Message; }
            return RedirectToAction("Theatres");
        }

        // ========== SCREENS ==========

        public async Task<IActionResult> Screens()
            => View(await _screenService.GetScreensAsync());

        public async Task<IActionResult> AddScreen()
            => View(new AddScreenViewModel
            {
                Theatres = await _theatreService.GetTheatresAsync()
            });

        [HttpPost]
        public async Task<IActionResult> AddScreen(AddScreenViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Theatres = await _theatreService.GetTheatresAsync();
                return View(vm);
            }
            await _screenService.AddScreenAsync(vm);
            return RedirectToAction("Theatres");
        }

        [HttpGet]
        public async Task<IActionResult> EditScreen(Guid id)
        {
            var screen = await _screenService.GetScreenByIdAsync(id);
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
                Theatres = await _theatreService.GetTheatresAsync()
            };
            ViewBag.ScreenId = id;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditScreen(Guid id, AddScreenViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Theatres = await _theatreService.GetTheatresAsync();
                ViewBag.ScreenId = id;
                return View(vm);
            }
            await _screenService.UpdateScreenAsync(id, vm);
            return RedirectToAction("Screens");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteScreen(Guid id)
        {
            try
            {
                await _screenService.DeleteScreenAsync(id);
                TempData["Success"] = "Screen deleted successfully";
            }
            catch (Exception ex) { TempData["Error"] = ex.Message; }
            return RedirectToAction("Screens");
        }

        [HttpGet]
        public async Task<IActionResult> GetScreensByTheatre(Guid theatreId)
        {
            if (theatreId == Guid.Empty) return Json(new List<object>());
            var screens = await _screenService.GetScreensByTheatreAsync(theatreId);
            return View(screens);
        }

        [HttpGet]
        public async Task<IActionResult> ScreensByTheatre(Guid theatreId)
        {
            if (theatreId == Guid.Empty) return Json(new List<object>());
            var screens = await _screenService.GetScreensByTheatreAsync(theatreId);
            return Ok(screens);
        }

        // ========== SHOWTIMES ==========

        public async Task<IActionResult> ShowTimes()
            => View(await _showTimeService.GetMovieWiseShowTimesAsync());

        [HttpGet]
        public async Task<IActionResult> AddShowTime()
            => View(await _showTimeService.GetAddShowTimeBulkFormAsync());

        [HttpPost]
        public async Task<IActionResult> AddShowTime(AddShowTimeBulkViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            await _showTimeService.AddShowTimesBulkAsync(vm);
            return RedirectToAction("ShowTimes");
        }

        [HttpGet]
        public async Task<IActionResult> EditShowTime(Guid id)
        {
            var showTime = await _showTimeService.GetShowTimeByIdAsync(id);
            var vm = new AddShowTimeBulkViewModel
            {
                Movies = await _movieService.GetMoviesAsync(),
                Theatres = await _theatreService.GetTheatresAsync(),
                Languages = await _languageService.GetLanguagesAsync()
            };
            ViewBag.ShowTimeId = id;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditShowTime(Guid id, AddShowTimeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Movies = await _movieService.GetMoviesAsync();
                vm.Theatres = await _theatreService.GetTheatresAsync();
                vm.Languages = await _languageService.GetLanguagesAsync();
                ViewBag.ShowTimeId = id;
                return View(vm);
            }
            await _showTimeService.UpdateShowTimeAsync(id, vm);
            return RedirectToAction("ShowTimes");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteShowTime(Guid id)
        {
            try
            {
                await _showTimeService.DeleteShowTimeAsync(id);
                TempData["Success"] = "ShowTime deleted successfully";
            }
            catch (Exception ex) { TempData["Error"] = ex.Message; }
            return RedirectToAction("ShowTimes");
        }

        // ========== LANGUAGES ==========

        public async Task<IActionResult> Languages()
            => View(await _languageService.GetLanguagesAsync());

        public IActionResult AddLanguage() => View();

        [HttpPost]
        public async Task<IActionResult> AddLanguage(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError("", "Language name is required");
                return View();
            }
            await _languageService.AddLanguageAsync(name);
            return RedirectToAction("Languages");
        }

        [HttpGet]
        public async Task<IActionResult> EditLanguage(Guid id)
        {
            var language = await _languageService.GetLanguageByIdAsync(id);
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
            await _languageService.UpdateLanguageAsync(id, name);
            return RedirectToAction("Languages");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLanguage(Guid id)
        {
            try
            {
                await _languageService.DeleteLanguageAsync(id);
                TempData["Success"] = "Language deleted successfully";
            }
            catch (Exception ex) { TempData["Error"] = ex.Message; }
            return RedirectToAction("Languages");
        }

        // ========== REQUESTS ==========

        public async Task<IActionResult> Requests()
            => View(await _requestApprovalService.GetRequestsAsync());

        public async Task<IActionResult> Approve(Guid id)
        {
            await _requestApprovalService.ApproveRequestAsync(id);
            return RedirectToAction("Requests");
        }

        public async Task<IActionResult> Reject(Guid id)
        {
            await _requestApprovalService.RejectRequestAsync(id);
            return RedirectToAction("Requests");
        }
    }
}