using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Web.Interfaces;
using MovieBooking.Web.Services;
using MovieBooking.Web.ViewModels;

namespace MovieBooking.Web.Controllers
{


    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminController : Controller
    {
        private readonly ISuperAdminMvcService _service;

        public SuperAdminController(ISuperAdminMvcService service)
        {
            _service = service;
        }
        public IActionResult Dashboard() => View();
        [HttpGet("movies")]
        public async Task<IActionResult> Movies()
            => View(await _service.GetMoviesAsync());

        public IActionResult AddMovie() => View();

        [HttpPost]
        public async Task<IActionResult> AddMovie(AddMovieViewModel vm)
        {
            await _service.AddMovieAsync(vm);
            return RedirectToAction("movies");
        }

        public async Task<IActionResult> Theatres()
        {
            return View(await _service.GetTheatresAsync());
        }
        public IActionResult AddTheatre() => View();

        [HttpPost]
        public async Task<IActionResult> AddTheatre(AddTheatreViewModel vm)
        {
            await _service.AddTheatreAsync(vm);
            return RedirectToAction("Theatres");
        }

        public async Task<IActionResult> AddScreen()
            => View(new AddScreenViewModel
            {
                Theatres = await _service.GetTheatresAsync()
            });

        [HttpPost]
        public async Task<IActionResult> AddScreen(AddScreenViewModel vm)
        {
            await _service.AddScreenAsync(vm);
            return RedirectToAction("Theatres");
        }
        [HttpGet]
        public async Task<IActionResult> GetScreensByTheatre(Guid theatreId)
        {
            if (theatreId == Guid.Empty)
                return Json(new List<object>());

            var screens = await _service.GetScreensByTheatreAsync(theatreId);
            return Json(screens);
        }

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
