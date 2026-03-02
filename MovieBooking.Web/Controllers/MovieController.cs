using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Web.Interfaces;
using MovieBooking.Web.ViewModels;

namespace MovieBooking.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class MovieController : Controller
    {
        private readonly IMovieMvcService MovieService;

        public MovieController(IMovieMvcService movieService)
        {
            MovieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
            => View(await MovieService.GetMoviesAsync());

        public IActionResult AddMovie() => View();

        [HttpPost]
        public async Task<IActionResult> AddMovie(AddMovieViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            try
            {
                await MovieService.AddMovieAsync(vm);
                TempData["Success"] = "Movie added successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditMovie(Guid id)
        {
            var movie = await MovieService.GetMovieByIdAsync(id);
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
            try
            {
                await MovieService.UpdateMovieAsync(id, vm);
                TempData["Success"] = "Movie updated successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(vm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMovie(Guid id)
        {
            try
            {
                await MovieService.DeleteMovieAsync(id);
                TempData["Success"] = "Movie deleted successfully";
            }
            catch (Exception ex) { TempData["Error"] = ex.Message; }
            return RedirectToAction("Index");
        }
    }
}