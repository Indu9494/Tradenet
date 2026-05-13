using Microsoft.EntityFrameworkCore;
using TradeNetAPI.Data;
using TradeNetAPI.Interfaces;
using TradeNetAPI.Models;

namespace TradeNetAPI.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(TradeNetDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId)
        {
            return await _dbSet.Where(n => n.UserID == userId).AsNoTracking().ToListAsync();
        }
    }
}
