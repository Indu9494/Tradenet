using TradeNet11.Models;

namespace TradeNet11.Interfaces
{
    public interface IComplianceNotificationRepository
    {
        Task<IEnumerable<ComplianceNotification>> GetAllAsync();
        Task<IEnumerable<ComplianceNotification>> GetUnreadByOfficerAsync(int officerId);
        Task<ComplianceNotification?> GetByIdAsync(int id);
        Task AddAsync(ComplianceNotification notification);
        Task MarkAsReadAsync(int id);
    }
}
