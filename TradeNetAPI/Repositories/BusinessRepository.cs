using Microsoft.EntityFrameworkCore;
using TradeNetAPI.Data;
using TradeNetAPI.Interfaces;
using TradeNetAPI.Models;

namespace TradeNetAPI.Repositories
{
    public class BusinessRepository : GenericRepository<Business>, IBusinessRepository
    {
        public BusinessRepository(TradeNetDbContext context) : base(context)
        {
        }

        public async Task<Business?> GetBusinessByUserIdAsync(int userId)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(b => b.UserID == userId);
        }

        public async Task<IEnumerable<Business>> GetBusinessesByUserIdAsync(int userId)
        {
            return await _dbSet.AsNoTracking().Where(b => b.UserID == userId).ToListAsync();
        }
    }
}
