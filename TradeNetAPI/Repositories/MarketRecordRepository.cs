using Microsoft.EntityFrameworkCore;
using TradeNetAPI.Data;
using TradeNetAPI.Interfaces;
using TradeNetAPI.Models;

namespace TradeNetAPI.Repositories
{
    public class MarketRecordRepository : GenericRepository<MarketRecord>, IMarketRecordRepository
    {
        public MarketRecordRepository(TradeNetDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MarketRecord>> GetMarketRecordsAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }
    }
}
