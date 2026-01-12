using Microsoft.AspNetCore.Mvc;
using MovieBooking.Web.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace MovieBooking.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> Logger;

        public HomeController(ILogger<HomeController> logger)
        {
            Logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Dashboard()
        {
            return Json(new
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                UserName = User.Identity.Name,
                Roles = User.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value)
            });
        }

    }
}
