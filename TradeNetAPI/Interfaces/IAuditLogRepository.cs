using TradeNetAPI.Models;

namespace TradeNetAPI.Interfaces;

public interface IAuditLogRepository : IRepository<AuditLog>
{
    Task<IEnumerable<AuditLog>> GetLogsByUserIdAsync(int userId);
}