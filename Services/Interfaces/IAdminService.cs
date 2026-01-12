using CommunityEvents.Models;
using CommunityEvents.Models.ViewModels;

namespace CommunityEvents.Services.Interfaces
{
    public interface IAdminService
    {
        Task<AdminDashboardViewModel> GetDashboardStatsAsync();
        IEnumerable<Review> GetAllReviews();
    }
}
