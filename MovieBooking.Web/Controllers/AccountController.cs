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

            var result = await AuthenticationApiService.LoginAsync(Model);

            if (result == null)
            {
                ModelState.AddModelError("", "Invalid credentials");
                return View(Model);
            }

            // Store JWT in secure cookie
            Response.Cookies.Append(
                "AuthToken",
                result.Token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,              // HTTPS only
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });

            // Store role (can also decode from JWT later)
            Response.Cookies.Append(
                "UserRole",
                result.Role,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });

            return RedirectToAction("LoginSuccess");
        }


        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel Model)
        {
            if (!ModelState.IsValid)
                return View(Model);

            var Result = await AuthenticationApiService.RegisterAsync(Model);

            if (Result == null)
            {
                ModelState.AddModelError("", "Registration failed. Email may already exist.");
                return View(Model);
            }

            return RedirectToAction("RegisterSuccess");
        }
        [HttpGet]
        public IActionResult RegisterSuccess()
        {
            return View();
        }
        [HttpGet]
        public IActionResult LoginSuccess()
        {
            return View();
        }
        public IActionResult Logout()
        {
            var cookieOptions = new CookieOptions
            {
                Path = "/",                  // MUST match creation
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Lax
            };

            Response.Cookies.Delete("AuthToken", cookieOptions);
            Response.Cookies.Delete("UserRole", cookieOptions);

            return RedirectToAction("Login", "Account");
        }

    }


}
