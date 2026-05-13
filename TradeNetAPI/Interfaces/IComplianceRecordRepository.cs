using TradeNetAPI.Models;

namespace TradeNetAPI.Interfaces;

public interface IComplianceRecordRepository : IRepository<ComplianceRecord>
{
    Task<IEnumerable<ComplianceRecord>> GetRecordsByBusinessIdAsync(int businessId);
}