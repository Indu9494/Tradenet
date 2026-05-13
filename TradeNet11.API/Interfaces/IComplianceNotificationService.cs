using TradeNet11.Models;

namespace TradeNet11.Interfaces
{
    public interface IComplianceNotificationService
    {
        Task<IEnumerable<ComplianceNotification>> GetAllNotificationsAsync();
        Task<IEnumerable<ComplianceNotification>> GetUnreadForOfficerAsync(int officerId);
        Task MarkAsReadAsync(int id);
    }
}
