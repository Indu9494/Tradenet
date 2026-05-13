using Microsoft.EntityFrameworkCore;
using TradeNetAPI.Data;
using TradeNetAPI.Interfaces;
using TradeNetAPI.Models;

namespace TradeNetAPI.Repositories
{
    public class ComplianceRecordRepository : GenericRepository<ComplianceRecord>, IComplianceRecordRepository
    {
        public ComplianceRecordRepository(TradeNetDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ComplianceRecord>> GetRecordsByBusinessIdAsync(int businessId)
        {
            return await _dbSet.AsNoTracking().Where(cr => cr.EntityID == businessId).ToListAsync();
        }
    }
}
