using CommunityEvents.Models;
using CommunityEvents.Models.ViewModels;
using CommunityEvents.Services.Interfaces;
using CommunityEvents.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CommunityEvents.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminService(IRepositoryWrapper repositoryWrapper, UserManager<IdentityUser> userManager)
        {
            _repositoryWrapper = repositoryWrapper;
            _userManager = userManager;
        }

        public async Task<AdminDashboardViewModel> GetDashboardStatsAsync()
        {
            var model = new AdminDashboardViewModel();

            // Get event count
            model.TotalEvents = _repositoryWrapper.EventRepository.FindAll().Count();

            // Get review count
            model.TotalReviews = _repositoryWrapper.ReviewRepository.FindAll().Count();

            // Get user count
            model.TotalUsers = await _userManager.Users.CountAsync();

            return model;
        }

        public IEnumerable<Review> GetAllReviews()
        {
            return _repositoryWrapper.ReviewRepository.FindAll()
                .Include(r => r.Event)
                .Include(r => r.User)
                .OrderByDescending(r => r.ReviewId)
                .ToList();
        }
    }
}
