using Microsoft.EntityFrameworkCore;
using TradeNetAPI.Data;
using TradeNetAPI.Interfaces;
using TradeNetAPI.Models;

namespace TradeNetAPI.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(TradeNetDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(int userId)
        {
            // Transaction does not have a UserID; join with Business to filter by the owning user
            var query = from t in _dbSet
                        join b in _context.Businesses on t.BusinessID equals b.BusinessID
                        where b.UserID == userId
                        select t;

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByBusinessIdAsync(int businessId)
        {
            return await _dbSet.Where(t => t.BusinessID == businessId).AsNoTracking().ToListAsync();
        }
    }
}
