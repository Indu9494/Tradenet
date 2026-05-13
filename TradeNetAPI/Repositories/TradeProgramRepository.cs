using Microsoft.EntityFrameworkCore;
using TradeNetAPI.Data;
using TradeNetAPI.Interfaces;
using TradeNetAPI.Models;

namespace TradeNetAPI.Repositories
{
    public class TradeProgramRepository : GenericRepository<TradeProgram>, ITradeProgramRepository
    {
        public TradeProgramRepository(TradeNetDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TradeProgram>> GetActiveProgramsAsync()
        {
            return await _dbSet.Where(p => p.Status == "Active").AsNoTracking().ToListAsync();
        }
    }
}
