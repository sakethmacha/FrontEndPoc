using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Web.Interfaces;

namespace MovieBooking.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class LanguageController : Controller
    {
        private readonly ILanguageMvcService LanguageService;

        public LanguageController(ILanguageMvcService languageService)
        {
            LanguageService = languageService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
            => View(await LanguageService.GetLanguagesAsync());

        public IActionResult AddLanguage() => View();

        [HttpPost]
        public async Task<IActionResult> AddLanguage(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError("", "Language name is required");
                return View();
            }
            try
            {
                await LanguageService.AddLanguageAsync(name);
                TempData["Success"] = "Language added successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditLanguage(Guid id)
        {
            var language = await LanguageService.GetLanguageByIdAsync(id);
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
            try
            {
                await LanguageService.UpdateLanguageAsync(id, name);
                TempData["Success"] = "Language updated successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLanguage(Guid id)
        {
            try
            {
                await LanguageService.DeleteLanguageAsync(id);
                TempData["Success"] = "Language deleted successfully";
            }
            catch (Exception ex) { TempData["Error"] = ex.Message; }
            return RedirectToAction("Index");
        }
    }
}