using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CommunityEvents.Models;
using CommunityEvents.Services.Interfaces;

namespace CommunityEvents.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly UserManager<IdentityUser> _userManager;

        public NotificationsController(INotificationService notificationService, UserManager<IdentityUser> userManager)
        {
            _notificationService = notificationService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var notifs = _notificationService.GetMyNotifications(user.Id);

            // Mark all as read when opening the page
            foreach (var n in notifs.Where(x => x.IsRead != true))
            {
                _notificationService.MarkAsRead(n.NotificationId);
            }
            return View(notifs);
        }
        public async Task<IActionResult> Settings()
        {
            var user = await _userManager.GetUserAsync(User);
            var prefs = _notificationService.GetMyPreferences(user.Id);
            return View(prefs);
        }

        [HttpPost]
        public async Task<IActionResult> Settings(NotificationPreference model)
        {
            var user = await _userManager.GetUserAsync(User);
            model.UserId = user.Id;

            _notificationService.UpdatePreferences(model);

            TempData["SuccessMessage"] = "Notification preferences updated successfully.";
            return RedirectToAction("Index");
        }

        //POST: Notifications/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            _notificationService.DeleteNotification(id, user.Id);

            return RedirectToAction("Index");
        }
    }
}
