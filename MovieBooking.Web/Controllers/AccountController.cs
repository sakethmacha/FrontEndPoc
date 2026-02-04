using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Web.Interfaces;
using MovieBooking.Web.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MovieBooking.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationMvcService AuthenticationService;

        public AccountController(IAuthenticationMvcService authenticationService)
        {
            AuthenticationService = authenticationService;
        }

        // ---------------- LOGIN (GET) ----------------
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // ---------------- LOGIN (POST) ----------------
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel,string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(loginViewModel);

            var result = await AuthenticationService.LoginAsync(loginViewModel);

            if (result == null)
            {
                ModelState.AddModelError("", "Invalid credentials");
                return View(loginViewModel);
            }

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(result.Token);

            var claims = jwt.Claims.ToList();

            //  STORE JWT FOR MVC → API CALLS
            claims.Add(new Claim("access_token", result.Token));

            claims.Add(new Claim(ClaimTypes.Name, result.Name));

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                });

            return RedirectToAction("LoginSuccess");
        }

        public IActionResult LoginSuccess()
        {
            return View();
        }


        // ---------------- REGISTER ----------------
        public IActionResult RegisterSuccess()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
                return View(registerViewModel);

            var result = await AuthenticationService.RegisterAsync(registerViewModel);

            if (result == null)
            {
                ModelState.AddModelError("", "Registration failed");
                return View(registerViewModel);
            }

            return RedirectToAction("RegisterSuccess");
        }

        // ---------------- LOGOUT ----------------
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
