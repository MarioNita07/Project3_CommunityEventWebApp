using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CommunityEvents.Services.Interfaces;

namespace CommunityEvents.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IReviewService _reviewService;

        public AdminController(IAdminService adminService, IReviewService reviewService, UserManager<IdentityUser> userManager)
        {
            _adminService = adminService;
            _reviewService = reviewService;
            _userManager = userManager;
        }

        // GET: Admin/Index
        public async Task<IActionResult> Index()
        {
            var stats = await _adminService.GetDashboardStatsAsync();
            return View(stats);
        }

        // GET: Admin/Reviews
        public IActionResult Reviews()
        {
            var allReviews = _adminService.GetAllReviews();
            return View(allReviews);
        }

        // POST: Admin/DeleteReview/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            _reviewService.DeleteReview(id, user.Id, isAdmin: true);

            TempData["SuccessMessage"] = "Review removed by moderator.";
            return RedirectToAction(nameof(Reviews));
        }
    }
}
