using TradeNetAPI.Models;

namespace TradeNetAPI.Interfaces;

public interface INotificationRepository : IRepository<Notification>
{
    Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId);
}