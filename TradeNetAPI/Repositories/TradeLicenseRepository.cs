using Microsoft.EntityFrameworkCore;
using TradeNetAPI.Data;
using TradeNetAPI.Interfaces;
using TradeNetAPI.Models;

namespace TradeNetAPI.Repositories
{
    public class TradeLicenseRepository : GenericRepository<TradeLicense>, ITradeLicenseRepository
    {
        public TradeLicenseRepository(TradeNetDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TradeLicense>> GetAvailableLicensesAsync()
        {
            return await _dbSet.Where(l => l.Status == "Available").AsNoTracking().ToListAsync();
        }

        public async Task<TradeLicense?> GetLicenseByBusinessIdAsync(int businessId)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(l => l.BusinessID == businessId);
        }
    }
}
