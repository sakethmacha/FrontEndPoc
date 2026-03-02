using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Web.Interfaces;
using MovieBooking.Web.ViewModels;

namespace MovieBooking.Web.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class AdminManagementController : Controller
    {
        private readonly IAdminManagementMvcService AdminService;

        public AdminManagementController(IAdminManagementMvcService adminService)
        {
            AdminService = adminService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
            => View(await AdminService.GetAdminsAsync());

        [HttpGet]
        public IActionResult CreateAdmin() => View();

        [HttpPost]
        public async Task<IActionResult> CreateAdmin(AddAdminViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            try
            {
                await AdminService.CreateAdminAsync(vm);
                TempData["Success"] = "Admin created successfully";
                return RedirectToAction("Index");
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
            var admin = await AdminService.GetAdminByIdAsync(id);
            var vm = new AddAdminViewModel
            {
                UserId = admin.UserId,
                Name = admin.Name,
                Email = admin.Email
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditAdmin(AddAdminViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            try
            {
                await AdminService.UpdateAdminAsync(vm.UserId, vm);
                TempData["Success"] = "Admin updated successfully";
                return RedirectToAction("Index");
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
                await AdminService.DeactivateAdminAsync(id);
                TempData["Success"] = "Admin deactivated successfully";
            }
            catch (Exception ex) { TempData["Error"] = ex.Message; }
            return RedirectToAction("Index");
        }
    }
}