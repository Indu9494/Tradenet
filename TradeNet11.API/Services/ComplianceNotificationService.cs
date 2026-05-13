using TradeNet11.Interfaces;
using TradeNet11.Models;

namespace TradeNet11.Services
{
    public class ComplianceNotificationService : IComplianceNotificationService
    {
        private readonly IComplianceNotificationRepository _notificationRepo;

        public ComplianceNotificationService(IComplianceNotificationRepository notificationRepo)
        {
            _notificationRepo = notificationRepo;
        }

        public async Task<IEnumerable<ComplianceNotification>> GetAllNotificationsAsync()
        {
            return await _notificationRepo.GetAllAsync();
        }

        public async Task<IEnumerable<ComplianceNotification>> GetUnreadForOfficerAsync(int officerId)
        {
            return await _notificationRepo.GetUnreadByOfficerAsync(officerId);
        }

        public async Task MarkAsReadAsync(int id)
        {
            await _notificationRepo.MarkAsReadAsync(id);
        }
    }
}
