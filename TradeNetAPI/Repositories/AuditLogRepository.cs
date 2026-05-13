using Microsoft.EntityFrameworkCore;
using TradeNetAPI.Data;
using TradeNetAPI.Interfaces;
using TradeNetAPI.Models;

namespace TradeNetAPI.Repositories
{
    public class AuditLogRepository : GenericRepository<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(TradeNetDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AuditLog>> GetLogsByUserIdAsync(int userId)
        {
            return await _dbSet.Where(a => a.UserID == userId).AsNoTracking().ToListAsync();
        }
    }
}
