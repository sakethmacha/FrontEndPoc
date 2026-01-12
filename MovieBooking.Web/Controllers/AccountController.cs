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
        private readonly IAuthenticationMvcService _authService;

        public AccountController(IAuthenticationMvcService authService)
        {
            _authService = authService;
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
        public async Task<IActionResult> Login(
            LoginViewModel model,
            string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            // 1️⃣ Call API
            var result = await _authService.LoginAsync(model);

            if (result == null)
            {
                ModelState.AddModelError("", "Invalid credentials");
                return View(model);
            }

            // 2️⃣ Decode JWT
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(result.Token);

            // 3️⃣ Extract claims from JWT
            var claims = jwt.Claims.ToList();

            // OPTIONAL: add Name claim for MVC UI
            claims.Add(new Claim(ClaimTypes.Name, result.Name));

            // 4️⃣ Create identity
            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            // 5️⃣ Sign in (COOKIE AUTH)
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                });

            // 6️⃣ Redirect to requested page
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
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.RegisterAsync(model);

            if (result == null)
            {
                ModelState.AddModelError("", "Registration failed");
                return View(model);
            }

            return RedirectToAction("Login");
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
