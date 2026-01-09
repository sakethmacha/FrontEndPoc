using Microsoft.AspNetCore.Mvc;
using MovieBooking.Web.Services;
using MovieBooking.Web.ViewModels;

namespace MovieBooking.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthenticationApiService AuthenticationApiService;

        public AccountController(AuthenticationApiService AuthenticationApiService)
        {
            this.AuthenticationApiService = AuthenticationApiService;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel Model)
        {
            if (!ModelState.IsValid)
                return View(Model);

            var Result = await AuthenticationApiService.LoginAsync(Model);

            if (Result == null)
            {
                ModelState.AddModelError("", "Invalid credentials");
                return View(Model);
            }

            HttpContext.Session.SetString("JwtToken", Result.Token);
            HttpContext.Session.SetString("UserRole", Result.Role);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Index() => View();
    }

}
